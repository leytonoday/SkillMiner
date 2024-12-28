using SkillMiner.Domain.Shared.ValueObjects;

namespace SkillMiner.Application.Abstractions.CommandQueue;

/// <summary>
/// Represents an input to the command queue. Exposes several methods for producers of command queue items.
/// </summary>
public interface ICommandQueueForProducer
{
    /// <summary>
    /// Writes a queued command to the underlying queue system.
    /// </summary>
    /// <returns>The TrackingId for the <see cref="CommandQueueMessage"/> to check it's status.</returns>
    Task<Guid> WriteAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken);

    Task<ProcessingStatus?> GetCommandQueueMessageProcessingStatusAsync(Guid trackingId, CancellationToken cancellationToken);

    Task<IDictionary<Guid, ProcessingStatus>> GetPendingAndProcessingAsync(CancellationToken cancellationToken);

    Task<IEnumerable<Guid>> GetFailedAsync(CancellationToken cancellationToken);
}
