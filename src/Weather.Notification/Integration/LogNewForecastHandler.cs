﻿using MediatR;
using Microsoft.Extensions.Logging;
using Weather.Forecast.Contract;
using Weather.Notification.Common.Cache;

namespace Weather.Notification.Integration;

internal sealed class LogNewForecastHandler(
    ILogger<LogNewForecastHandler> logger,
    IMeteorologistCache meteorologistCache)
    : INotificationHandler<WeatherForecastCreated>
{
    public async Task Handle(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        var meteorologist = await meteorologistCache.GetByIdAsync(notification.MeteorologistId);

        logger.LogInformation(
            "A {ForecastNotificationSummary} forecast was published at {ForecastNotificationCreationDate}{MeteorologistInfo}",
            notification.Summary ?? "", notification.DateCreated,
            meteorologist == null ? "" : $" by {meteorologist.Fullname}");
    }
}