using MediatR;
using Microsoft.Extensions.Logging;
using Weather.Forecast.Contract;

namespace Weather.Notification.Integration;

internal sealed class NotifyNewForecastHandler(ILogger<NotifyNewForecastHandler> logger) : INotificationHandler<WeatherForecastCreated>
{
    public Task Handle(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("a {ForecastNotificationSummary} forecast was publish at {ForecastNotificationCreationDate}", notification.Summary, notification.DateCreated);
        return Task.CompletedTask;
    }
}
