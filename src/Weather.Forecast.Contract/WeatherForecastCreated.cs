using MediatR;

namespace Weather.Forecast.Contract;

public sealed record WeatherForecastCreated(string? Summary) : INotification
{
    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.Now;
}
