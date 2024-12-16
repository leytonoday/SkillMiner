using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Domain.Entities.WebScrapingTaskEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.JobListingEntity.Queue;

public sealed record WebScrapeJobsByTitleCommandQueuedCommand(string JobTitle, WebScrapingTaskId WebScrapingTaskId) : QueuedCommand;

public sealed class WebScrapeJobsByTitleCommandQueuedCommandHandler
    (IWebScrapingTaskRepository webScrapingTaskRepository,
    IUnitOfWork unitOfWork
    )
    : IQueuedCommandHandler<WebScrapeJobsByTitleCommandQueuedCommand>
{
    public async Task Handle(WebScrapeJobsByTitleCommandQueuedCommand request, CancellationToken cancellationToken)
    {
        var webScrapingTask = await webScrapingTaskRepository.GetByIdAsync(request.WebScrapingTaskId, cancellationToken)
            ?? throw new Exception("WebScrapingTaskJob Not Found");

        webScrapingTask.MarkAsStarted();
        await unitOfWork.CommitAsync(cancellationToken);

        try
        {
            //var jobListing = JobListing.CreateNew("TITLE",
            //    "COMAPNY",
            //    "LOCATION",
            //    "DESC",
            //    "URL",
            //    DateTime.UtcNow,
            //    "USD",
            //    EmploymentType.Apprenticeship);

            //await jobListingRepository.AddAsync(jobListing, cancellationToken);
            //await unitOfWork.CommitAsync(cancellationToken);

            webScrapingTask.MarkAsCompleted();
        } catch (Exception ex)
        {
            webScrapingTask.MarkAsFailed(ex.Message);
        }
        await unitOfWork.CommitAsync(cancellationToken);
    }
}