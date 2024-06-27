using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Contract;

public sealed record MeteorologistPromoted(Guid Id, string Prestige) : IIntegrationEvent;