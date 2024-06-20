using Weather.Forecast.Features.Meteorologists.Domain;
using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Features.Forecasts.Domain;

internal sealed class WeatherForecast : Entity<WeatherForecastId>, IAggregateRoot
{
    private WeatherForecast(DateOnly date, Temperature temperature, MeteorologistId? meteorologistId)
    {
        Id = WeatherForecastId.NewWeatherForecastId;
        Date = date;
        Temperature = temperature;
        Summary = GetSummary(temperature);
        MeteorologistId = meteorologistId;
        AddDomainEvent(new WeatherForecastCreated(Summary, meteorologistId));
    }

    private static Summary? GetSummary(Temperature temperature)
    {
        return temperature.Celsius switch
        {
            >= -56 and < -42 => Domain.Summary.Bracing,
            >= -42 and < -28 => Domain.Summary.Chilly,
            >= -28 and < -14 => Domain.Summary.Cool,
            >= -14 and < 0 => Domain.Summary.Mild,
            >= 0 and < 14 => Domain.Summary.Warm,
            >= 14 and < 28 => Domain.Summary.Balmy,
            >= 28 and < 42 => Domain.Summary.Hot,
            >= 42 and < 58 => Domain.Summary.Sweltering,
            > 58 => Domain.Summary.Scorching,
            _ => null
        };
    }

    public DateOnly Date { get; private set; }
    public Temperature Temperature { get; private set; }
    public Summary? Summary { get; private set; }
    public MeteorologistId? MeteorologistId { get; private set; }

    internal static WeatherForecast Create(DateOnly date, Temperature temperature,
        MeteorologistId? meteorologistId = null) =>
        new(date, temperature, meteorologistId == Guid.Empty ? null : meteorologistId);
}