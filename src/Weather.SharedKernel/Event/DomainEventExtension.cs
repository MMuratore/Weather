using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.SharedKernel.Domain;

namespace Weather.SharedKernel.Event;

internal static class DomainEventExtension
{
    internal static async Task PublishDomainEventsAsync(IPublisher publisher, DbContext? dbContext, CancellationToken cancellationToken)
    {
        if (dbContext is null) return;

        var entities = dbContext.ChangeTracker.Entries<IHasDomainEvents>()
            .Where(entry => entry.Entity.DomainEvents.Count != 0).Select(entry => entry.Entity).ToList();

        var domainEvents = entities.SelectMany(x => x.DomainEvents).ToList();

        entities.ForEach(entity => entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents) await publisher.Publish(domainEvent, cancellationToken);
    }
}