﻿using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.JobListingEntity;
using SkillMiner.Infrastructure.CommandQueue;
using SkillMiner.Infrastructure.Persistence.EntityConfiguration;

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
}
