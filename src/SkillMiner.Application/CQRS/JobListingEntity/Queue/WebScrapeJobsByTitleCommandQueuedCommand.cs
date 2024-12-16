using SkillMiner.Application.Abstractions.CommandQueue;

namespace SkillMiner.Application.CQRS.JobListingEntity.Queue;

public sealed record WebScrapeJobsByTitleCommandQueuedCommand(string JobTitle): QueuedCommand;

public sealed class WebScrapeJobsByTitleCommandQueuedCommandHandler : IQueuedCommandHandler<WebScrapeJobsByTitleCommandQueuedCommand>
{
    public Task Handle(WebScrapeJobsByTitleCommandQueuedCommand request, CancellationToken cancellationToken)
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

        return Task.CompletedTask;
    }
}