using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Entities.ProfessionEntity;
using SkillMiner.Infrastructure.Shared;

namespace SkillMiner.Infrastructure.Persistence.EntityConfiguration.ProfessionConfiguration;

internal sealed class ProfessionKeywordConfiguration : IEntityTypeConfiguration<ProfessionKeyword>
{
    public void Configure(EntityTypeBuilder<ProfessionKeyword> builder)
    {
        builder.ToTable(nameof(ProfessionKeyword));

        // Make sure the Profession Strongly Typed Id has a conversion
        builder.Property(e => e.ProfessionId)
          .HasConversion(id => id.Value, value => new ProfessionId(value))
          .IsRequired();

        // The combination of the profession id, keyword and created date should be unique.
        // That is to say, a keyword can only be assigned to a profession at a date only once.
        builder.HasKey(x => new { x.ProfessionId, x.Keyword, x.CreatedOnUtc });

        // Configure a many to one relationship between professions and keywords.
        builder.HasOne(x => x.Profession)
            .WithMany(x => x.Keywords)
            .HasPrincipalKey(x => x.Id);
    }
}

