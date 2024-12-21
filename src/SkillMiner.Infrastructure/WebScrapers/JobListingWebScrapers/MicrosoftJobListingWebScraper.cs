using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using SkillMiner.Application.Services.WebScraper;
using SkillMiner.Application.Shared.Results;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;
using SkillMiner.Domain.Entities.WebScrapingTaskEntity;
using SkillMiner.Infrastructure.WebScrapers.WebScraperHelper;
using System.Text;
using System.Web;

namespace SkillMiner.Infrastructure.WebScrapers.JobListingWebScraper;

internal partial class MicrosoftJobListingWebScraper
    (IWebScraperHelper webScraperHelper,
    IMicrosoftJobListingRepository microsoftJobListingRepository,
    ILogger<MicrosoftJobListingWebScraper> logger)
    : IJobListingWebScraper<MicrosoftJobListing>
{
    [System.Text.RegularExpressions.GeneratedRegex(@"^Job item \d+$")]
    private static partial System.Text.RegularExpressions.Regex JobItemRegex();

    public async Task<Result<IEnumerable<MicrosoftJobListing>>> ScrapeAsync(JobListingWebScraperInput input, CancellationToken cancellationToken)
    {
        List<int> alreadyScrapedJobItemNumbers = await microsoftJobListingRepository.GetAllJobNumbersAsync(cancellationToken);

        IEnumerable<int> newJobItemNumbers = await GetJobItemNumbersFromMicrosoftAsync(input.JobTitle, 2, alreadyScrapedJobItemNumbers, cancellationToken);

        IEnumerable<MicrosoftJobListing> newJobListings = await GetJobDataAsync(newJobItemNumbers, input.WebScrapingTaskId, cancellationToken);

        return Result.Success(newJobListings);
    }

    /// <summary>
    /// The Microsoft job board identified job listings with "Job Item Numbers" or "Job Numbers". This method will scrape all Job Item Numbers from
    /// the Microsoft job board. 
    /// </summary>
    /// <param name="jobTitle">The job title to search the job board for.</param>
    /// <param name="webScrapingTaskId">The Id of the <see cref="WebScrapingTask"/> that started this Webscraping.</param>
    /// <param name="maxPages">The upper limit of pages to search for new job listings for.</param>
    /// <param name="alreadyScrapedJobItemNumbers">A list of already webscraped job item numbers.</param>
    /// <param name="cancellationToken">Cancels the operation.</param>
    /// <returns>A list of job item numbers that have not been webscraped previously.</returns>
    async Task<IEnumerable<int>> GetJobItemNumbersFromMicrosoftAsync(string jobTitle, int maxPages, List<int> alreadyScrapedJobItemNumbers, CancellationToken cancellationToken)
    {
        int pageNumber = 1;

        // We search by Recent job listings first. So if we come across any Job Item Numbers that have already been webscraped, then we'll know that it's time to stop.
        string BuildUrl(int pageNumber, string jobTitle) => "https://jobs.careers.microsoft.com/global/en/search?q=" + Uri.EscapeDataString(jobTitle) + "&l=en_us&pg=" + pageNumber + "&pgSz=20&o=Recent";

        IEnumerable<int> ExtractJobItemNumbersFromHtmlDocument(HtmlDocument document)
        {
            // XPath to find all divs with aria-label starting with "Job item" followed by a number
            var jobNodes = document.DocumentNode.SelectNodes("//div[starts-with(@aria-label, 'Job item ')]");

            if (jobNodes == null || jobNodes.Count == 0)
            {
                logger.LogInformation("No job postings found.");
                throw new WebScrapingException("Could not find Job Nodes");
            }

            // Use a regular expression to extract valid "Job item <number>" strings
            var regex = JobItemRegex();

            // Collect all aria-label attributes that match the pattern
            var jobLabels = jobNodes
                .Select(node => node.GetAttributeValue("aria-label", ""))
                .Where(label => regex.IsMatch(label))
                .ToList();

            if (jobLabels.Count == 0)
            {
                logger.LogInformation("No matching job postings found.");
                return [];
            }

            // "Job Item 1234" -> 1234
            return jobLabels.Select(x => int.Parse(x.Split(" ")[2]));
        }

        var allJobItemNumbers = new HashSet<int>();

        try
        {
            while (true)
            {
                string url = BuildUrl(pageNumber, jobTitle);

                string? htmlContent = await webScraperHelper.ReadWebpageToHtmlStringAsync(url, 1500, cancellationToken);
                if (htmlContent is null)
                {
                    continue;
                }

                HtmlDocument document = webScraperHelper.StringToHtmlDocument(htmlContent);

                // Step 3: Extract job postings
                IEnumerable<int> jobItemNumbers = ExtractJobItemNumbersFromHtmlDocument(document);
                if (!jobItemNumbers.Any())
                {
                    break;
                }

                // Add any job item numbers that haven't already been webscraped
                foreach (int jobItemNumber in jobItemNumbers.Where(x => !alreadyScrapedJobItemNumbers.Contains(x)))
                {
                    allJobItemNumbers.Add(jobItemNumber);
                }

                // If we come across any job item numbers in this page that have already been webscraped, then we know that we've
                // exhausted the new job listings, because we're sorting by "Recent". We break here, presuming that we've already
                // scraped everything past this point.
                if (alreadyScrapedJobItemNumbers.Intersect(jobItemNumbers).Any())
                {
                    break;
                }

                logger.LogInformation("Page " + pageNumber + ", Added: " + string.Join(", ", jobItemNumbers));

                pageNumber++;
                if (pageNumber > maxPages)
                {
                    break;
                }
            }

            logger.LogInformation(string.Join(", ", allJobItemNumbers));
        }
        catch (Exception ex)
        {
            logger.LogInformation($"An error occurred: {ex.Message}");
        }

        return [.. allJobItemNumbers];
    }


    async Task<IEnumerable<MicrosoftJobListing>> GetJobDataAsync(IEnumerable<int> jobItemNumbers, WebScrapingTaskId webScrapingTaskId, CancellationToken cancellationToken)
    {
        string baseUrl = "https://jobs.careers.microsoft.com/global/en/job";

        var newJobListings = new List<MicrosoftJobListing>();

        MicrosoftJobListing? ExtractJobDataFromHtml(HtmlDocument document, int jobItemNumber, string url)
        {
            HtmlNode? searchJobDetailsCardNode = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'SearchJobDetailsCard')]");
            if (searchJobDetailsCardNode is null)
            {
                return null;
            }

            // Get job title
            HtmlNode? jobTitleNode = searchJobDetailsCardNode.SelectSingleNode(".//h1");
            string jobTitle = jobTitleNode.InnerText;

            // Get Location
            HtmlNode? locationNode = searchJobDetailsCardNode.ChildNodes[2];
            string location = locationNode.InnerText;

            // Get job info table
            HtmlNode? jobInfoTableNode = searchJobDetailsCardNode.ChildNodes
                .Where(node => node.NodeType == HtmlNodeType.Element) // Ignore non-element nodes (e.g., text or comments)
                .ElementAtOrDefault(4); // Index 4 corresponds to the 5th element

            var jobInfo = new Dictionary<string, string>();

            if (jobInfoTableNode is not null)
            {
                foreach (var child in jobInfoTableNode.ChildNodes)
                {
                    string infoType = child.ChildNodes[0].ChildNodes[0].InnerText;
                    string infoData = child.ChildNodes[0].ChildNodes[1].InnerText;

                    jobInfo.Add(infoType, infoData);
                }
            }

            // Get Overview, Qualifications and Resposibilities
            HtmlNode? overviewAndQualificationsWrapperNode = searchJobDetailsCardNode.ChildNodes
                .Where(node => node.NodeType == HtmlNodeType.Element) // Ignore non-element nodes (e.g., text or comments)
                .ElementAtOrDefault(6); // Index 6 corresponds to the 7th element#
            if (overviewAndQualificationsWrapperNode is null)
            {
                return null;
            }

            // Overview
            HtmlNode? overviewWrapperNode = overviewAndQualificationsWrapperNode.ChildNodes[0];
            string overview = "";
            if (overviewWrapperNode is not null)
            {
                overview = overviewWrapperNode.ChildNodes[0].ChildNodes[1].InnerText.Replace("&nbsp;", "\n\n");
            }

            // Qualifications
            HtmlNode? qualificationsWrapperNode = overviewAndQualificationsWrapperNode.ChildNodes[1];
            string qualifications = "";
            if (overviewWrapperNode is not null)
            {
                qualifications = qualificationsWrapperNode.ChildNodes[0].ChildNodes[1].InnerText.Replace("&nbsp;", "\n\n");
            }

            // Responsibilities
            HtmlNode? responsibilitiesWrapperNode = overviewAndQualificationsWrapperNode.ChildNodes[2];
            string responsibilities = "";
            if (responsibilitiesWrapperNode is not null)
            {
                responsibilities = responsibilitiesWrapperNode.ChildNodes[0].ChildNodes[1].InnerText.Replace("&nbsp;", "\n\n");
            }

            // Benefits
            string benefits = "";
            var benefitsBuilder = new StringBuilder();
            HtmlNode? benefitsWrapperNode = searchJobDetailsCardNode.ChildNodes[8];
            if (benefitsWrapperNode is not null)
            {
                benefitsBuilder.AppendLine(benefitsWrapperNode.ChildNodes[0].InnerText);

                HtmlNode? benefitsList = benefitsWrapperNode.ChildNodes[1];
                if (benefitsList is not null)
                {
                    foreach (var benefitNode in benefitsList.ChildNodes)
                    {
                        benefitsBuilder.AppendLine(benefitNode.InnerText);
                    }
                }
            }
            benefits = benefitsBuilder.ToString();

            return MicrosoftJobListing.CreateNew(
                webScrapingTaskId,
                jobTitle,
                jobItemNumber,
                url,
                location,
                DateTime.ParseExact(jobInfo["Date posted"], "MMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture),
                jobInfo["Work site"],
                jobInfo["Travel"],
                jobInfo["Role type"],
                jobInfo["Profession"],
                jobInfo["Discipline"],
                jobInfo["Employment type"],
                overview,
                qualifications,
                responsibilities,
                benefits);
        }

        foreach (int jobItemNumber in jobItemNumbers)
        {
            string url = $"{baseUrl}/{jobItemNumber}";

            string? htmlContent = await webScraperHelper.ReadWebpageToHtmlStringAsync(url, 3000, cancellationToken);
            if (htmlContent is null)
            {
                continue;
            }

            HtmlDocument document = webScraperHelper.StringToHtmlDocument(htmlContent);

            MicrosoftJobListing? jobListing = ExtractJobDataFromHtml(document, jobItemNumber, url);

            if (jobListing is not null)
            {
                newJobListings.Add(jobListing);
                logger.LogInformation("JobListing Created for Job Item Number {number}", jobItemNumber);
            }

            // Wait for a few seconds to prevent rate limiting.
            await Task.Delay(2000, cancellationToken);
        }

        return newJobListings;
    }
}
