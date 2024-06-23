using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Contract;

public sealed record MeteorologistCreated(Guid MeteorologistId, string Fullname) : IIntegrationEvent
{
    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.Now;
}