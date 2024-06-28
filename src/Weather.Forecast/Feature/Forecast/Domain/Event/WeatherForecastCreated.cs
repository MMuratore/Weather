using Weather.Forecast.Feature.Forecast.Domain.ValueObject;
using Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;
using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Feature.Forecast.Domain.Event;

internal sealed record WeatherForecastCreated(Summary? Summary, MeteorologistId? MeteorologistId) : IDomainEvent;

internal static class WeatherForecastCreatedMapper
{
    public static Contract.WeatherForecastCreated ToIntegrationEvent(this WeatherForecastCreated @event) =>
        new(@event.Summary?.ToString(), (Guid?)@event.MeteorologistId);
}