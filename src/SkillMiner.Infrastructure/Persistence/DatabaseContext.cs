using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.JobListingEntity;
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

    public DbSet<JobListing> JobListings { get; set; }

    public DbSet<WebScrapingTask> WebScrapingTasks { get; set; }
}
