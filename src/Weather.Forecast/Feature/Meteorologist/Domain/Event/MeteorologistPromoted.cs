using Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;
using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Feature.Meteorologist.Domain.Event;

internal sealed record MeteorologistPromoted(MeteorologistId Id, Prestige Prestige) : IDomainEvent;

internal static class MeteorologistForecastCountIncrementedMapper
{
    public static Contract.MeteorologistPromoted ToIntegrationEvent(this MeteorologistPromoted @event) =>
        new((Guid)@event.Id, @event.Prestige.ToString());
}