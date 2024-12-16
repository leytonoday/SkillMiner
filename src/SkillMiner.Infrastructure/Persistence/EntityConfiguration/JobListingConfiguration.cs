﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.JobListingEntity;
using SkillMiner.Infrastructure.Shared;
using SkillMiner.Domain.Entities.WebScrapingTaskEntity;

namespace SkillMiner.Infrastructure.Persistence.EntityConfiguration;

internal sealed class JobListingConfiguration : IEntityTypeConfiguration<JobListing>
{
    public void Configure(EntityTypeBuilder<JobListing> builder)
    {
        builder.ToTable(nameof(JobListing));

        #region Configure Shadow Property Database Id
        builder.Property<int>(Constants.DatabaseIdColumnName)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(Constants.DatabaseIdColumnName);
        #endregion

        #region Configure Strongly Typed Domain Id
        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, value => new JobListingId(value))
            .IsRequired();

        builder.HasIndex(x => x.Id)
            .IsClustered(false)
            .HasDatabaseName($"IX_{nameof(JobListing)}_Id")
            .IsUnique();
        #endregion

        builder.Property(e => e.WebScrapingTaskId)
           .HasConversion(id => id.Value, value => new WebScrapingTaskId(value))
            .IsRequired();
    }
}

