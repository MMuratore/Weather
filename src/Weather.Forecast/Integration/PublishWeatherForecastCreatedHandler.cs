using MediatR;
using Weather.Forecast.Features.Forecasts.Domain;
using Weather.Forecast.Persistence;

namespace Weather.Forecast.Integration;

internal sealed class PublishWeatherForecastCreatedHandler(ForecastDbContext dbContext)
    : INotificationHandler<WeatherForecastCreated>
{
    public async Task Handle(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        await dbContext.AddIntegrationEventAsync(notification.ToIntegrationEvent(), cancellationToken);
    }
}