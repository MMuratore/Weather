﻿using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Contract;

public sealed record WeatherForecastCreated(string? Summary, Guid? MeteorologistId) : IIntegrationEvent
{
    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.Now;
}