using Weather.Forecast.Features.Meteorologists.Domain;
using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Features.Forecasts.Domain;

internal sealed record WeatherForecastCreated(Summary? Summary, MeteorologistId? MeteorologistId) : IDomainEvent;

internal static class WeatherForecastCreatedMapper
{
    public static Contract.WeatherForecastCreated ToIntegrationEvent(this WeatherForecastCreated @event) =>
        new(@event.Summary?.ToString(), (Guid?)@event.MeteorologistId);
}