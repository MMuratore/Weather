using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Contract;

public sealed record MeteorologistCreated(Guid Id, string Fullname) : IIntegrationEvent
{
    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.Now;
}