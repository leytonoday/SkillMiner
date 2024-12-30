using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Infrastructure.Shared;
using SkillMiner.Domain.Entities.ProfessionEntity;

namespace SkillMiner.Infrastructure.Persistence.EntityConfiguration.ProfessionConfiguration;

internal sealed class ProfessionConfiguration : IEntityTypeConfiguration<Profession>
{
    public void Configure(EntityTypeBuilder<Profession> builder)
    {
        builder.ToTable(nameof(Profession));

        #region Configure Shadow Property Database Id
        builder.Property<int>(Constants.DatabaseIdColumnName)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(Constants.DatabaseIdColumnName);
        #endregion

        #region Configure Strongly Typed Domain Id
        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, value => new ProfessionId(value))
            .IsRequired();

        builder.HasIndex(x => x.Id)
            .IsClustered(false)
            .HasDatabaseName($"IX_{nameof(Profession)}_Id")
            .IsUnique();
        #endregion

        // Make the name unique for all professions
        builder.HasIndex(x => x.Name)
            .IsClustered(false)
            .HasDatabaseName($"IX_{nameof(Profession)}_Name")
            .IsUnique();
    }
}

