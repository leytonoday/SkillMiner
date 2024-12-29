using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;

namespace SkillMiner.Application.CQRS.Queue;

public record GenerateSkillsByProfessionQueuedCommand : QueuedCommand;

public class GenerateSkillsByProfessionQueuedCommandHandler(
    IMicrosoftJobListingRepository microsoftJobListingRepository
    ) : IQueuedCommandHandler<GenerateSkillsByProfessionQueuedCommand>
{
    public async Task Handle(GenerateSkillsByProfessionQueuedCommand request, CancellationToken cancellationToken)
    {
        var requirementsByProfession = await GetRequirementsByProfessionAsync(cancellationToken);
    }

    public async Task<IDictionary<string, List<string>>> GetRequirementsByProfessionAsync(CancellationToken cancellationToken)
    {
        var requirementsByProfession = new Dictionary<string, List<string>>();

        // Get the 50 most recent microsoft job listings
        var recentMicrosoftJobListings = (await microsoftJobListingRepository.GetPageAsync(0, 50, cancellationToken)).Item1;

        return recentMicrosoftJobListings
            .Where(x => x.Profession is not null && x.Qualifications is not null)
            .GroupBy(x => x.Profession)
            .ToDictionary(
                group => group.Key!,
                group => group.Select(listing => listing.Qualifications!).ToList()
            );
    }
}