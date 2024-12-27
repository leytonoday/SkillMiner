using Microsoft.EntityFrameworkCore;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Domain.Shared.ValueObjects;
using SkillMiner.Infrastructure.Persistence;

namespace SkillMiner.Infrastructure.CommandQueue;

/// <inheritdoc cref="ICommandQueueForProducer"/>
public class CommandQueueForWriter(DatabaseContext databaseContext) : ICommandQueueForProducer 
{
    private DbSet<CommandQueueMessage> GetDbSet() => databaseContext.Set<CommandQueueMessage>();

    /// <inheritdoc/>
    public Task<Guid> WriteAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var set = GetDbSet();

        var commandQueueMessage = CommandQueueMessage.CreateFrom(queuedCommand);

        set.Add(commandQueueMessage);

        return Task.FromResult(commandQueueMessage.TrackingId);
    }

    public async Task<ProcessingStatus?> GetCommandQueueMessageProcessingStatusAsync(Guid trackingId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var set = GetDbSet();

        var commandQueueMessage = await set.FirstOrDefaultAsync(x => x.TrackingId == trackingId, cancellationToken);

        return commandQueueMessage?.ProcessingStatus;
    }
}
