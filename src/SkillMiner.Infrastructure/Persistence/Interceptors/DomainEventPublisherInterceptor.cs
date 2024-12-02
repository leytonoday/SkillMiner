using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SkillMiner.Domain.Shared.Entities;
using SkillMiner.Domain.Shared.Events;

namespace SkillMiner.Infrastructure.Persistence.Interceptors;


/// <summary>
/// Intercepts database context when saving changes, and publishes all domain events.
/// </summary>
/// <param name="publisher">The MediatR <see cref="IPublisher"/> used to publish domain events.</param>
public sealed class DomainEventPublisherInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return await base.SavingChangesAsync(
                eventData,
                result,
                cancellationToken);
        }

        // Extract domain events from all entities within the change tracker.
        IEnumerable<IDomainEvent> domainEvents = dbContext.ChangeTracker
            .Entries<IEntity>()
            .Select(x => x.Entity)
            .SelectMany(entity => entity.DomainEvents);

        // Iterate over each domain event and publish them sequentially
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }

        // Clear all domain events
        dbContext.ChangeTracker
            .Entries<IEntity>()
            .Select(x => x.Entity)
            .ToList()
            .ForEach(x => x.ClearDomainEvents());

        return await base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }
}
