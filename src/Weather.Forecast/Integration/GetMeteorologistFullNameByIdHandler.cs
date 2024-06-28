using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Contract;
using Weather.Forecast.Feature.Meteorologist.Domain;

namespace Weather.Forecast.Integration;

internal sealed class
    GetMeteorologistFullNameByIdHandler(ForecastDbContext dbContext)
    : IRequestHandler<GetMeteorologistFullNameById, MeteorologistFullname?>
{
    public async Task<MeteorologistFullname?> Handle(GetMeteorologistFullNameById request,
        CancellationToken cancellationToken)
    {
        var meteorologist = await dbContext.Set<Meteorologist>()
            .FirstOrDefaultAsync(x => x.Id == request.MeteorologistId, cancellationToken);

        return meteorologist is null
            ? null
            : new MeteorologistFullname((Guid)meteorologist.Id, meteorologist.Name.Fullname);
    }
}