using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Contract;

public sealed record WeatherForecastCreated(string? Summary) : IIntegrationEvent
{
    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.Now;
}