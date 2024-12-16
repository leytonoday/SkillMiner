using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.Services.WebScraper;
using SkillMiner.Application.Shared.Results;
using SkillMiner.Domain.Entities.JobListingEntity;
using SkillMiner.Domain.Entities.WebScrapingTaskEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.JobListingEntity.Queue;

public sealed record WebScrapeJobsByTitleQueuedCommand(string JobTitle, WebScrapingTaskId WebScrapingTaskId) : QueuedCommand;

public sealed class WebScrapeJobsByTitleQueuedCommandHandler
    (IWebScrapingTaskRepository webScrapingTaskRepository,
    IJobListingRepository jobListingRepository,
    IEnumerable<IJobListingWebScraper> jobListingWebScrapers,
    IUnitOfWork unitOfWork)
    : IQueuedCommandHandler<WebScrapeJobsByTitleQueuedCommand>
{
    public async Task Handle(WebScrapeJobsByTitleQueuedCommand request, CancellationToken cancellationToken)
    {
        var webScrapingTask = await webScrapingTaskRepository.GetByIdAsync(request.WebScrapingTaskId, cancellationToken)
            ?? throw new Exception("WebScrapingTaskJob Not Found");

        webScrapingTask.MarkAsStarted();
        await unitOfWork.CommitAsync(cancellationToken);

        try
        {
            if (!jobListingWebScrapers.Any())
            {
                throw new Exception("No IJobListingWebScraper implementations available.");
            }

            foreach (IJobListingWebScraper webScraper in jobListingWebScrapers)
            {
                var result = await webScraper.ScrapeAsync(new JobListingWebScraperInput(request.JobTitle));
                
                foreach (Result<JobListing> jobListingResult in result.Where(x => x.Data is not null))
                {
                    if (jobListingResult.Data is not null)
                    {
                        continue;
                    }

                    await jobListingRepository.AddAsync(jobListingResult.Data, cancellationToken);
                }
            }

            webScrapingTask.MarkAsCompleted();
        } catch (Exception ex)
        {
            webScrapingTask.MarkAsFailed(ex.Message);
        }
        await unitOfWork.CommitAsync(cancellationToken);
    }
}