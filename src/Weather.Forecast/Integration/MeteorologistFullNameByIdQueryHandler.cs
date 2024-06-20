using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Contract;
using Weather.Forecast.Features.Meteorologists.Domain;
using Weather.Forecast.Persistence;

namespace Weather.Forecast.Integration;

internal sealed class MeteorologistFullNameByIdQueryHandler : IRequestHandler<MeteorologistFullNameByIdQuery, MeteorologistFullname?>
{
    private readonly ForecastDbContext _dbContext;

    public MeteorologistFullNameByIdQueryHandler(ForecastDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MeteorologistFullname?> Handle(MeteorologistFullNameByIdQuery request,
        CancellationToken cancellationToken)
    {
        var meteorologist = await _dbContext.Set<Meteorologist>()
            .FirstOrDefaultAsync(x => x.Id == request.MeteorologistId, cancellationToken: cancellationToken);

        return meteorologist is null? null : new MeteorologistFullname((Guid)meteorologist.Id, meteorologist.Name.Fullname);
    }
}