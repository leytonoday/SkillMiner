using MediatR;
using SkillMiner.Domain.Shared.Events;

namespace SkillMiner.Application.Abstractions;

/// <summary>
/// Represents an event handler for a <see cref="IDomainEvent"/>.
/// </summary>
/// <typeparam name="TEvent">The type of <see cref="IDomainEvent"/> to handle.</typeparam>
public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent;
