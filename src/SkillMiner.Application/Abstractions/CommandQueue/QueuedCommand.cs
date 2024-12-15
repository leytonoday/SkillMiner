using MediatR;

namespace SkillMiner.Application.Abstractions.CommandQueue;

/// <summary>
/// Represents a command that can be stored within the module's Queue.
/// </summary>
public abstract record QueuedCommand : IRequest
{
    /// <summary>
    /// Gets the unique identifier for the queued command.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();
}