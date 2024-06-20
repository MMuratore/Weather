using MediatR;
using Weather.Forecast.Features.Meteorologists.Domain;
using Weather.Forecast.Persistence;

namespace Weather.Forecast.Integration;

internal sealed class PublishMeteorologistCreatedHandler(ForecastDbContext dbContext)
    : INotificationHandler<MeteorologistCreated>
{
    public async Task Handle(MeteorologistCreated notification, CancellationToken cancellationToken)
    {
        await dbContext.AddIntegrationEventAsync(notification.ToIntegrationEvent(), cancellationToken);
    }
}