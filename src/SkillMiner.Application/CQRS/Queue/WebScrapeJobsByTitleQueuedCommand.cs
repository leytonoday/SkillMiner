using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Application.Services.WebScraper;
using SkillMiner.Domain.Entities.BackgroundTaskEntity;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.Queue;

public sealed record WebScrapeJobsByTitleQueuedCommand(string JobTitle, BackgroundTaskId BackgroundTaskId) : QueuedCommand;

public sealed class WebScrapeJobsByTitleQueuedCommandHandler
    (IBackgroundTaskRepository backgroundTaskRepository,
    IJobListingWebScraper<MicrosoftJobListing> microsoftJobListingWebScaper,
    IMicrosoftJobListingRepository microsoftJobListingRepository,
    IUnitOfWork unitOfWork)
    : IQueuedCommandHandler<WebScrapeJobsByTitleQueuedCommand>
{
    public async Task Handle(WebScrapeJobsByTitleQueuedCommand request, CancellationToken cancellationToken)
    {
        var backgroundTask = await backgroundTaskRepository.GetByIdAsync(request.BackgroundTaskId, cancellationToken)
            ?? throw new Exception("BackgroundTask Not Found");

        backgroundTask.MarkAsStarted();
        await unitOfWork.CommitAsync(cancellationToken);

        try
        {
            var webScraperInput = new JobListingWebScraperInput(backgroundTask.Id, request.JobTitle);

            var result = await microsoftJobListingWebScaper.ScrapeAsync(webScraperInput, cancellationToken);

            // Failed for any reason.
            if (!result.IsSuccess)
            {
                backgroundTask.MarkAsFailed();
                return;
            }

            // Succeeded, but there are no new job listings to scrape
            if (result.IsSuccess && (result.Data is null || !result.Data.Any()))
            {
                backgroundTask.MarkAsCompleted();
                return;
            }

            // Succeeded and there are new job listings. Add to repo.
            foreach (var jobListing in result.Data!)
            {
                await microsoftJobListingRepository.AddAsync(jobListing, cancellationToken);
            }
            backgroundTask.MarkAsCompleted();
        }
        catch
        {
            // If an exception is thrown for whatever reason (maybe a webscraping issue), then just mark the task as failed and re-throw so that the Command Queue Message Processor can try again.
            backgroundTask.MarkAsFailed();
            throw;
        }
        finally
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}