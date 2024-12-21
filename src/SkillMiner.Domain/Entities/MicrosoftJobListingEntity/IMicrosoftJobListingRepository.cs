using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Domain.Entities.MicrosoftJobListingEntity;

public interface IMicrosoftJobListingRepository : IRepository<MicrosoftJobListing, MicrosoftJobListingId>
{
    public Task<List<int>> GetAllJobNumbersAsync(CancellationToken cancellationToken);
}