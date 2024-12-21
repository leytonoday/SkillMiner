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
}