using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.WebScrapingTaskEntity;
using SkillMiner.Infrastructure.Shared;

namespace SkillMiner.Infrastructure.Persistence.EntityConfiguration;

internal sealed class WebScrapingTaskConfiguration : IEntityTypeConfiguration<WebScrapingTask>
{
    public void Configure(EntityTypeBuilder<WebScrapingTask> builder)
    {
        builder.ToTable(nameof(WebScrapingTask));

        #region Configure Shadow Property Database Id
        builder.Property<int>(Constants.DatabaseIdColumnName)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(Constants.DatabaseIdColumnName);
        #endregion

        #region Configure Strongly Typed Domain Id
        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, value => new WebScrapingTaskId(value))
            .IsRequired();

        builder.HasIndex(x => x.Id)
            .IsClustered(false)
            .HasDatabaseName($"IX_{nameof(WebScrapingTask)}_Id")
            .IsUnique();
        #endregion
    }
}

