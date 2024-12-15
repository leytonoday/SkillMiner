using Microsoft.EntityFrameworkCore;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Infrastructure.Persistence;

namespace SkillMiner.Infrastructure.CommandQueue;

/// <inheritdoc cref="ICommandQueueWriter"/>
public class CommandQueueWriter(DatabaseContext databaseContext) : ICommandQueueWriter 
{
    private DbSet<CommandQueueMessage> GetDbSet() => databaseContext.Set<CommandQueueMessage>();

    /// <inheritdoc/>
    public Task WriteAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var set = GetDbSet();

        set.Add(CommandQueueMessage.CreateFrom(queuedCommand));

        return Task.CompletedTask;
    }
}
