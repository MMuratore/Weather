using MediatR;
using Microsoft.Extensions.Logging;
using Weather.Forecast.Contract;

namespace Weather.Notification.Integration;

internal sealed class LogMeteorologistPromotionHandler(
    ILogger<LogMeteorologistPromotionHandler> logger,
    IMeteorologistCache meteorologistCache)
    : INotificationHandler<MeteorologistPromoted>
{
    public async Task Handle(MeteorologistPromoted notification, CancellationToken cancellationToken)
    {
        var meteorologist = await meteorologistCache.GetByIdAsync(notification.Id);

        logger.LogInformation("{MeteorologistInfo} was promoted to {ForecastNotificationCreationDate}", meteorologist == null ? $"{notification.Id}" : $"{meteorologist.Fullname}",notification.Prestige);
    }
}