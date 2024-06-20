using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Features.Meteorologists.Domain;

internal sealed record MeteorologistCreated(MeteorologistId Id, string Fullname) : IDomainEvent;

internal static class MeteorologistCreatedMapper
{
    public static Contract.MeteorologistCreated ToIntegrationEvent(this MeteorologistCreated @event) =>
        new((Guid)@event.Id, @event.Fullname);
}