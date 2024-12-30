using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.ProfessionEntity;

namespace SkillMiner.Infrastructure.Persistence;

public sealed class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }

    public DbSet<Profession> Professions { get; set; }

    public DbSet<ProfessionKeyword> ProfessionKeywords { get; set; }
}
