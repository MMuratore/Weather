﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Features.Forecasts.Domain;
using Weather.Forecast.Persistence;
using Weather.SharedKernel.Domain;
using Weather.SharedKernel.Event;

namespace Weather.Forecast.Features.Meteorologists.Domain;

internal sealed class ForecastRecordByHandler(IPublisher publisher, ForecastDbContext dbContext)
    : DomainEventHandler<WeatherForecastCreated>(publisher, dbContext)
{
    protected override async Task Publish(WeatherForecastCreated notification, CancellationToken cancellationToken)
    {
        if (notification.MeteorologistId is null) return;

        var meteorologist = await dbContext.Set<Meteorologist>()
            .FirstOrDefaultAsync(x => x.Id == notification.MeteorologistId, cancellationToken);

        if (meteorologist is null) throw new DomainException("meteorologist not found");

        meteorologist.IncrementForecast();
    }
}