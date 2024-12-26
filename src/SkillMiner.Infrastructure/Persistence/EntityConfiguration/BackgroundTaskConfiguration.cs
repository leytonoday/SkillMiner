using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.BackgroundTaskEntity;
using SkillMiner.Infrastructure.Shared;

namespace SkillMiner.Infrastructure.Persistence.EntityConfiguration;

internal sealed class BackgroundTaskConfiguration : IEntityTypeConfiguration<BackgroundTask>
{
    public void Configure(EntityTypeBuilder<BackgroundTask> builder)
    {
        builder.ToTable(nameof(BackgroundTask));

        #region Configure Shadow Property Database Id
        builder.Property<int>(Constants.DatabaseIdColumnName)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(Constants.DatabaseIdColumnName);
        #endregion

        #region Configure Strongly Typed Domain Id
        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, value => new BackgroundTaskId(value))
            .IsRequired();

        builder.HasIndex(x => x.Id)
            .IsClustered(false)
            .HasDatabaseName($"IX_{nameof(BackgroundTask)}_Id")
            .IsUnique();
        #endregion
    }
}

