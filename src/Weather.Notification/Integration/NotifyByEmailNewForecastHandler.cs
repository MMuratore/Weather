using FluentEmail.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Weather.Forecast.Contract;

namespace Weather.Notification.Integration;

internal sealed class NotifyByEmailNewForecastHandler(
    ILogger<NotifyByEmailNewForecastHandler> logger,
    IMeteorologistCache meteorologistCache,
    IFluentEmail fluentEmail)
    : INotificationHandler<WeatherForecastCreated>
{
    private const string To = "random@acme.com";

    public async Task Handle(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        var meteorologist = await meteorologistCache.GetByIdAsync(notification.MeteorologistId);

        var response = await fluentEmail
            .To(To)
            .Subject("A new forecast was published")
            .Body(ForecastNotificationBody(notification, meteorologist))
            .SendAsync();

        if (response.Successful) return;

        logger.LogError("An error occurred while sending an email to {to} with the following messages: {@Messages}", To,
            response.ErrorMessages);
    }

    private static string ForecastNotificationBody(WeatherForecastCreated weatherForecast, Meteorologist? meteorologist)
    {
        var publishBy = meteorologist is null ? "" : $" by {meteorologist.Fullname}";
        return $"A {weatherForecast.Summary} forecast was published at {weatherForecast.DateCreated}{publishBy}";
    }
}