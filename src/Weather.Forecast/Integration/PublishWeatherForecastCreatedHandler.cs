using MediatR;
using Weather.Forecast.Domain;

namespace Weather.Forecast.Integration;

internal sealed class PublishWeatherForecastCreatedHandler(IPublisher publisher)
    : INotificationHandler<WeatherForecastCreated>
{
    public async Task Handle(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        await publisher.Publish(notification.ToIntegrationEvent(), cancellationToken);
    }
}
