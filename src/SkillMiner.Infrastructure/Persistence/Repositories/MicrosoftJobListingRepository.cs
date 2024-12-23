using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;

namespace SkillMiner.Infrastructure.Persistence.Repositories;

public sealed class MicrosoftJobListingRepository(DatabaseContext context)
    : Repository<MicrosoftJobListing, DatabaseContext, MicrosoftJobListingId>(context), IMicrosoftJobListingRepository
{
    public Task<List<int>> GetAllJobNumbersAsync(CancellationToken cancellationToken)
    {
        IQueryable<MicrosoftJobListing> query = DbSet;

        return DbSet.Select(x => x.JobNumber).ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<MicrosoftJobListing>, int)> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        IQueryable<MicrosoftJobListing> query = DbSet;

        int total = await DbSet.CountAsync(cancellationToken);

        IEnumerable<MicrosoftJobListing> paginatedEntities = await DbSet
            .OrderBy(x => x.DatePosted)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (paginatedEntities, total);
    }
}