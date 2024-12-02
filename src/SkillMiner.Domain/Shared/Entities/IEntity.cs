using System.ComponentModel.DataAnnotations.Schema;
using SkillMiner.Domain.Shared.Events;

namespace SkillMiner.Domain.Shared.Entities;

/// <summary>
/// Represents an entity that doesn't have a single primary key.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Collection of domain events associated with the entity.
    /// </summary>
    [NotMapped]
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Adds a domain event to the collection, triggering a notification handler elsewhere for this domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to be added.</param>
    void AddDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Removes a domain event from the collection.
    /// </summary>
    /// <param name="domainEvent">The domain event to be removed.</param>
    void RemoveDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Clears all application events associated with the entity.
    /// </summary>
    void ClearDomainEvents();
}

/// <summary>
/// Represents an entity that has a single primary key.
/// </summary>
/// <typeparam name="TId">The type of the primary key of this entity.</typeparam>
public interface IEntity<TId> : IEntity where TId : EntityId
{
    /// <summary>
    /// Id of the <see cref="IEntity{TId}"/>.
    /// </summary>
    public TId Id { get; }
}