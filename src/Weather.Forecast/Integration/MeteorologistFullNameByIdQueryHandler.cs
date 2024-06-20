using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Contract;
using Weather.Forecast.Features.Meteorologists.Domain;
using Weather.Forecast.Persistence;

namespace Weather.Forecast.Integration;

internal sealed class
    MeteorologistFullNameByIdQueryHandler(ForecastDbContext dbContext)
    : IRequestHandler<MeteorologistFullNameByIdQuery, MeteorologistFullname?>
{
    public async Task<MeteorologistFullname?> Handle(MeteorologistFullNameByIdQuery request,
        CancellationToken cancellationToken)
    {
        var meteorologist = await dbContext.Set<Meteorologist>()
            .FirstOrDefaultAsync(x => x.Id == request.MeteorologistId, cancellationToken);

        return meteorologist is null
            ? null
            : new MeteorologistFullname((Guid)meteorologist.Id, meteorologist.Name.Fullname);
    }
}