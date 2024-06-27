using MediatR;
using Weather.Forecast.Features.Forecasts.Domain;
using Weather.Forecast.Persistence;
using Weather.SharedKernel.Event;

namespace Weather.Forecast.Integration;

internal sealed class PublishWeatherForecastCreatedHandler(IPublisher publisher, ForecastDbContext dbContext)
    : DomainEventHandler<WeatherForecastCreated>(publisher, dbContext)
{
    protected override async Task Publish(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        await dbContext.AddIntegrationEventAsync(notification.ToIntegrationEvent(), cancellationToken);
    }
}