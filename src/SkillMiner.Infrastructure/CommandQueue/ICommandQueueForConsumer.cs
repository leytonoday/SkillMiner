using SkillMiner.Application.Abstractions.CommandQueue;

namespace SkillMiner.Infrastructure.CommandQueue;

/// <summary>
/// Represents an output to the command queue. Exposes several methods required for the consumption of command queue items.
/// </summary>
public interface ICommandQueueForConsumer
{
    /// <summary>
    /// Retrieves a list of unprocessed messages from the queue.
    /// </summary>
    public Task<List<CommandQueueMessage>> ListPendingAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Marks a specified queue message as starting to be processed.
    /// </summary>
    /// <param name="commandQueueMessage">The queue message to mark as processed.</param>
    public Task MarkStartedAsync(CommandQueueMessage commandQueueMessage, CancellationToken cancellationToken);

    /// <summary>
    /// Marks a specified queue message as processed.
    /// </summary>
    /// <param name="commandQueueMessage">The queue message to mark as processed.</param>
    public Task MarkProcessedAsync(CommandQueueMessage commandQueueMessage, CancellationToken cancellationToken);

    /// <summary>
    /// Marks a specified queue message as failed.
    /// </summary>
    /// <param name="commandQueueMessage">The queue message to mark as failed.</param>
    /// <param name="errorMessage">The error that caused the queue message to fail processing.</param>
    public Task MarkFailedAsync(CommandQueueMessage commandQueueMessage, string errorMessage, CancellationToken cancellationToken);
}
