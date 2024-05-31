using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Weather.SharedKernel.Domain;

namespace Weather.SharedKernel.Persistence;

public sealed class PublishDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _publisher;
    
    public PublishDomainEventsInterceptor(IPublisher publisher) => _publisher = publisher;
    
    
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        await PublishDomainEventsAsync(eventData.Context, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    private async Task PublishDomainEventsAsync(DbContext? dbContext, CancellationToken cancellationToken)
    {
        if (dbContext is null) return;
        
        var entities = dbContext.ChangeTracker.Entries<IHasDomainEvents>()
            .Where(entry => entry.Entity.DomainEvents.Count != 0).Select(entry => entry.Entity).ToList();
        
        var domainEvents = entities.SelectMany(x => x.DomainEvents).ToList();
        
        entities.ForEach(entity => entity.ClearDomainEvents());
        
        foreach (var domainEvent in domainEvents) await _publisher.Publish(domainEvent, cancellationToken);
    }
}