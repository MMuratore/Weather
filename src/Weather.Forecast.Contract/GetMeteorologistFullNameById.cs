using MediatR;

namespace Weather.Forecast.Contract;

public sealed record GetMeteorologistFullNameById(Guid MeteorologistId) : IRequest<MeteorologistFullname?>;

public sealed record MeteorologistFullname(Guid Id, string Fullname);