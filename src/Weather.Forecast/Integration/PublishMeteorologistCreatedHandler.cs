using MediatR;
using Weather.Forecast.Features.Meteorologists.Domain;
using Weather.Forecast.Persistence;
using Weather.SharedKernel.Event;

namespace Weather.Forecast.Integration;

internal sealed class PublishMeteorologistCreatedHandler(IPublisher publisher, ForecastDbContext dbContext)
    : DomainEventHandler<MeteorologistCreated>(publisher, dbContext)
{
    protected override async Task Publish(MeteorologistCreated notification, CancellationToken cancellationToken)
    {
        await dbContext.AddIntegrationEventAsync(notification.ToIntegrationEvent(), cancellationToken);
    }
}