using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.Services.WebScraper;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;
using SkillMiner.Domain.Entities.WebScrapingTaskEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.JobListingEntity.Queue;

public sealed record WebScrapeJobsByTitleQueuedCommand(string JobTitle, WebScrapingTaskId WebScrapingTaskId) : QueuedCommand;

public sealed class WebScrapeJobsByTitleQueuedCommandHandler
    (IWebScrapingTaskRepository webScrapingTaskRepository,
    IJobListingWebScraper<MicrosoftJobListing> microsoftJobListingWebScaper,
    IMicrosoftJobListingRepository microsoftJobListingRepository,
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
            var webScraperInput = new JobListingWebScraperInput(webScrapingTask.Id, request.JobTitle);

            var result = await microsoftJobListingWebScaper.ScrapeAsync(webScraperInput, cancellationToken);

            // Failed for any reason.
            if (!result.IsSuccess)
            {
                webScrapingTask.MarkAsFailed();
                return;
            }

            // Succeeded, but there are no new job listings to scrape
            if (result.IsSuccess && (result.Data is null || !result.Data.Any())) 
            {
                webScrapingTask.MarkAsCompleted();
                return;
            }

            // Succeeded and there are new job listings. Add to repo.
            foreach (var jobListing in result.Data!)
            {
                await microsoftJobListingRepository.AddAsync(jobListing, cancellationToken);
            }
            webScrapingTask.MarkAsCompleted();
        } catch
        {
            // If an exception is thrown for whatever reason (maybe a webscraping issue), then just mark the task as failed and re-throw so that the Command Queue Message Processor can try again.
            webScrapingTask.MarkAsFailed();
            throw;
        }
        finally
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}