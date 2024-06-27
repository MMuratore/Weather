using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Weather.SharedKernel.Event;

namespace Weather.SharedKernel.Persistence;

public sealed class PublishDomainEventsInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        await DomainEventExtension.PublishDomainEventsAsync(publisher, eventData.Context, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}