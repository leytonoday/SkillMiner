using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Infrastructure.Shared;
using SkillMiner.Domain.Entities.MicrosoftJobListingEntity;
using SkillMiner.Domain.Entities.BackgroundTaskEntity;

namespace SkillMiner.Infrastructure.Persistence.EntityConfiguration;

internal sealed class MicrosoftJobListingConfiguration : IEntityTypeConfiguration<MicrosoftJobListing>
{
    public void Configure(EntityTypeBuilder<MicrosoftJobListing> builder)
    {
        builder.ToTable(nameof(MicrosoftJobListing));

        #region Configure Shadow Property Database Id
        builder.Property<int>(Constants.DatabaseIdColumnName)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(Constants.DatabaseIdColumnName);
        #endregion

        #region Configure Strongly Typed Domain Id
        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, value => new MicrosoftJobListingId(value))
            .IsRequired();

        builder.HasIndex(x => x.Id)
            .IsClustered(false)
            .HasDatabaseName($"IX_{nameof(MicrosoftJobListing)}_Id")
            .IsUnique();
        #endregion

        builder.Property(e => e.BackgroundTaskId)
            .HasConversion(id => id.Value, value => new BackgroundTaskId(value))
            .IsRequired();
    }
}

