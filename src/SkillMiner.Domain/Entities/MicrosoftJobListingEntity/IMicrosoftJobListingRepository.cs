using SkillMiner.Domain.Shared.Persistence;

namespace SkillMiner.Domain.Entities.MicrosoftJobListingEntity;

public interface IMicrosoftJobListingRepository : IRepository<MicrosoftJobListing, MicrosoftJobListingId>
{
    public Task<List<int>> GetAllJobNumbersAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets a page of <see cref="MicrosoftJobListing"/>.
    /// </summary>
    /// <param name="pageNumber">The page of Job listings to read.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="cancellationToken">Cancels the operation.</param>
    /// <returns>The paginated results, as well as the total number of results to indicate if there are more to come.</returns>
    public Task<(IEnumerable<MicrosoftJobListing>, int)> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}