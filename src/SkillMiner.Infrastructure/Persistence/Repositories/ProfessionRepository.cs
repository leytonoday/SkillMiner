using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.ProfessionEntity;

namespace SkillMiner.Infrastructure.Persistence.Repositories;

public sealed class ProfessionRepository(DatabaseContext context)
    : Repository<Profession, DatabaseContext, ProfessionId>(context), IProfessionRepository
{
    public async Task<Profession?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await DbSet
            .Include(x => x.Keywords)
            .FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
    }
}