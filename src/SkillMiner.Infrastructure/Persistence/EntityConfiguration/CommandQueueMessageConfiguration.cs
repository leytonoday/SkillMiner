using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Infrastructure.Shared;
using SkillMiner.Domain.Shared.ValueObjects;
using SkillMiner.Application.Abstractions.CommandQueue;

namespace SkillMiner.Infrastructure.Persistence.EntityConfiguration;

internal sealed class CommandQueueMessageConfiguration : IEntityTypeConfiguration<CommandQueueMessage>
{
    public void Configure(EntityTypeBuilder<CommandQueueMessage> builder)
    {
        builder.ToTable(nameof(CommandQueueMessage));

        #region Configure Shadow Property Database Id
        builder.Property<int>(Constants.DatabaseIdColumnName)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(Constants.DatabaseIdColumnName);
        #endregion

        #region Configure Strongly Typed Domain Id
        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, value => new CommandQueueMessageId(value))
            .IsRequired();

        builder.HasIndex(x => x.Id)
            .IsClustered(false)
            .HasDatabaseName($"IX_{nameof(CommandQueueMessage)}_Id")
            .IsUnique();
        #endregion

        builder.HasIndex(x => x.TrackingId)
            .IsClustered(false)
            .HasDatabaseName($"IX_{nameof(CommandQueueMessage)}_TrackingId")
            .IsUnique();
    }
}

