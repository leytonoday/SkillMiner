using MediatR;
using SkillMiner.Domain.Entities.JobListingEntity;
using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Application.CQRS.JobListingEntity.Commands;

public record GenerateDesiredSkillsByJobTitleCommand(string JobTitle) : IRequest;

public class GenerateDesiredSkillsByJobTitleCommandHandler(
    IJobListingRepository jobListingRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<GenerateDesiredSkillsByJobTitleCommand>
{
    public async Task Handle(GenerateDesiredSkillsByJobTitleCommand request, CancellationToken cancellationToken)
    {
        var jobListing = JobListing.CreateNew("TITLE",
            "COMAPNY",
            "LOCATION",
            "DESC",
            "URL",
            DateTime.UtcNow,
            "USD",
            EmploymentType.Apprenticeship);

        await jobListingRepository.AddAsync(jobListing, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}