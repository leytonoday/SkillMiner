using MediatR;

namespace SkillMiner.Domain.Shared.Events;

/// <summary>
/// Represents a domain event, which captures something notable that has happened within the domain in the past,
/// but that is a side effect and shouldn't be handled there and then in the code. But it should be handled within the same database transaction.
/// </summary>
public interface IDomainEvent : INotification;

