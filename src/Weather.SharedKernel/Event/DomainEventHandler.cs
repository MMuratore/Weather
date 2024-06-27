using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.SharedKernel.Domain;

namespace Weather.SharedKernel.Event;

public abstract class DomainEventHandler<TDomainEvent>(IPublisher publisher, DbContext dbContext)
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        await Publish(notification, cancellationToken);
        await DomainEventExtension.PublishDomainEventsAsync(publisher, dbContext, cancellationToken);
    }

    protected abstract Task Publish(TDomainEvent notification, CancellationToken cancellationToken);
}