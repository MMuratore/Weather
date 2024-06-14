using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Features.Forecasts.Domain;
using Weather.Forecast.Persistence;

namespace Weather.Forecast.Features.Meteorologists.Domain;

internal sealed class ForecastRecordByHandler(ForecastDbContext dbContext)
    : INotificationHandler<WeatherForecastCreated>
{
    public async Task Handle(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        if (notification.MeteorologistId is null) return;

        var meteorologist = await dbContext.Set<Meteorologist>()
            .FirstOrDefaultAsync(x => x.Id == notification.MeteorologistId, cancellationToken);

        if (meteorologist is null) throw new Exception("meteorologist not found");

        meteorologist.IncrementForecast();
    }
}