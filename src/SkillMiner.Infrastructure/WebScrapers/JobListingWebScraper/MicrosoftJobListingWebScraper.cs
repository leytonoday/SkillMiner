using SkillMiner.Application.Services.WebScraper;
using SkillMiner.Application.Shared.Results;
using SkillMiner.Domain.Entities.JobListingEntity;

namespace SkillMiner.Infrastructure.WebScrapers.JobListingWebScraper;

internal class MicrosoftJobListingWebScraper : IJobListingWebScraper
{
    public Task<IEnumerable<Result<JobListing>>> ScrapeAsync(JobListingWebScraperInput input)
    {
        throw new NotImplementedException();
    }
}
