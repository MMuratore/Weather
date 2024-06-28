using Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;
using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Feature.Meteorologist.Domain.Event;

internal sealed record MeteorologistCreated(MeteorologistId Id, string Fullname) : IDomainEvent;

internal static class MeteorologistCreatedMapper
{
    public static Contract.MeteorologistCreated ToIntegrationEvent(this MeteorologistCreated @event) =>
        new((Guid)@event.Id, @event.Fullname);
}