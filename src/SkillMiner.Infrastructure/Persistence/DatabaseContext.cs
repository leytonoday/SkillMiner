using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;
using SkillMiner.Domain.Entities.WebScrapingTaskEntity;
using SkillMiner.Infrastructure.CommandQueue;

namespace SkillMiner.Infrastructure.Persistence;

public sealed class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        modelBuilder.AddCommandMessageQueue();
    }

    public DbSet<MicrosoftJobListing> MicrosoftJobListings { get; set; }

    public DbSet<WebScrapingTask> WebScrapingTasks { get; set; }
}
