using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Forecast.Domain.Event;
using Weather.Forecast.Feature.Meteorologist.Domain.Error;
using Weather.SharedKernel.Domain.Exception;
using Weather.SharedKernel.Event;

namespace Weather.Forecast.Feature.Meteorologist.Domain.Handler;

internal sealed class ForecastRecordByHandler(IPublisher publisher, ForecastDbContext dbContext)
    : DomainEventHandler<WeatherForecastCreated>(publisher, dbContext)
{
    protected override async Task Publish(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        if (notification.MeteorologistId is null) return;

        var meteorologist = await dbContext.Set<Meteorologist>()
            .FirstOrDefaultAsync(x => x.Id == notification.MeteorologistId, cancellationToken);

        if (meteorologist is null) throw new DomainNotFoundException(nameof(notification.MeteorologistId),MeteorologistError.MeteorologistNotFound);

        meteorologist.IncrementForecast();
    }
}