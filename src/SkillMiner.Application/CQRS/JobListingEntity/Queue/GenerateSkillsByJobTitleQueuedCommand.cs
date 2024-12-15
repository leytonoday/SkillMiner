using SkillMiner.Application.Abstractions.CommandQueue;

namespace SkillMiner.Application.CQRS.JobListingEntity.Queue;

public sealed record GenerateSkillsByJobTitleQueuedCommand(string JobTitle): QueuedCommand;

public sealed class GenerateSkillsByJobTitleQueuedCommandHandler : IQueuedCommandHandler<GenerateSkillsByJobTitleQueuedCommand>
{
    public Task Handle(GenerateSkillsByJobTitleQueuedCommand request, CancellationToken cancellationToken)
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

        throw new NotImplementedException();
    }
}