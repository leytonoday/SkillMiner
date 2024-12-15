using MediatR;

namespace SkillMiner.Application.Abstractions.CommandQueue;

/// <summary>
/// Represents a handler for a queued command.
/// </summary>
/// <typeparam name="TQueuedCommand">The type of the queued command this handler can process. 
/// Must inherit from <see cref="QueuedCommand"/>.</typeparam>
public interface IQueuedCommandHandler<in TQueuedCommand> : IRequestHandler<TQueuedCommand>
    where TQueuedCommand : QueuedCommand;