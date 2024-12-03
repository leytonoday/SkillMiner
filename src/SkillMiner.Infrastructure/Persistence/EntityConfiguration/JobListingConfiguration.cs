using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.JobListingEntity;

namespace SkillMiner.Infrastructure.Persistence.EntityConfiguration;

internal sealed class JobListingConfiguration : IEntityTypeConfiguration<JobListing>
{
    public void Configure(EntityTypeBuilder<JobListing> builder)
    {
        builder.ToTable(nameof(JobListing));

        #region Configure Database Id
        const string databaseIdName = $"DatabaseId";

        builder.Property<int>(databaseIdName)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(databaseIdName);
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
    }
}

