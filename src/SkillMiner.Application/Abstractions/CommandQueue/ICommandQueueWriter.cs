namespace SkillMiner.Application.Abstractions.CommandQueue;

/// <summary>
/// Represents an input to the command queue.
/// </summary>
public interface ICommandQueueWriter
{
    /// <summary>
    /// Writes a queued command to the underlying queue system.
    /// </summary>
    Task WriteAsync(QueuedCommand queuedCommand, CancellationToken cancellationToken);
}
