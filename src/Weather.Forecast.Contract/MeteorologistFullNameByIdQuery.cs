using MediatR;

namespace Weather.Forecast.Contract;

public sealed record MeteorologistFullNameByIdQuery(Guid MeteorologistId) : IRequest<MeteorologistFullname?>;