using MediatR;
using Weather.Forecast.Features.Meteorologists.Domain;
using Weather.Forecast.Persistence;

namespace Weather.Forecast.Integration;

internal sealed class PublishMeteorologistPromotedHandler(ForecastDbContext dbContext)
    : INotificationHandler<MeteorologistPromoted>
{
    public async Task Handle(MeteorologistPromoted notification, CancellationToken cancellationToken)
    {
        await dbContext.AddIntegrationEventAsync(notification.ToIntegrationEvent(), cancellationToken);
    }
}