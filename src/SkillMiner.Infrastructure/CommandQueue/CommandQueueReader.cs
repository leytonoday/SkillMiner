using Microsoft.EntityFrameworkCore;
using SkillMiner.Application.Abstractions.CommandQueue;
using SkillMiner.Infrastructure.Persistence;

namespace SkillMiner.Infrastructure.CommandQueue;

/// <inheritdoc cref="ICommandQueueReader"/>
public class CommandQueueReader(DatabaseContext databaseContext) : ICommandQueueReader
{
    private DbSet<CommandQueueMessage> GetDbSet() => databaseContext.Set<CommandQueueMessage>();

    /// <inheritdoc/>
    public async Task<List<CommandQueueMessage>> ListPendingAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        return await query
            .AsQueryable<CommandQueueMessage>()
            .Where(x => x.ProcessedOnUtc == null && x.Error == null)
            .OrderBy(x => x.CreatedOnUtc)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task MarkProcessedAsync(CommandQueueMessage commandQueueMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        commandQueueMessage.MarkProcessed();
        query.Update(commandQueueMessage);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task MarkFailedAsync(CommandQueueMessage commandQueueMessage, string errorMessage, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = GetDbSet();

        commandQueueMessage.SetError(errorMessage);
        query.Update(commandQueueMessage);

        return Task.CompletedTask;
    }
}
