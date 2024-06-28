using MediatR;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Meteorologist.Domain.Event;
using Weather.SharedKernel.Event;

namespace Weather.Forecast.Integration;

internal sealed class PublishMeteorologistPromotedHandler(IPublisher publisher, ForecastDbContext dbContext)
    : DomainEventHandler<MeteorologistPromoted>(publisher, dbContext)
{
    protected override async Task Publish(MeteorologistPromoted notification, CancellationToken cancellationToken)
    {
        await dbContext.AddIntegrationEventAsync(notification.ToIntegrationEvent(), cancellationToken);
    }
}