using Weather.Forecast.Feature.Forecast.Domain.Event;
using Weather.Forecast.Feature.Forecast.Domain.ValueObject;
using Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;
using Weather.SharedKernel.Domain;

namespace Weather.Forecast.Feature.Forecast.Domain;

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

    public DateOnly Date { get; private set; }
    public Temperature Temperature { get; private set; }
    public Summary? Summary { get; private set; }
    public MeteorologistId? MeteorologistId { get; private set; }

    private static Summary? GetSummary(Temperature temperature)
    {
        return temperature.Celsius switch
        {
            >= -56 and < -42 => ValueObject.Summary.Bracing,
            >= -42 and < -28 => ValueObject.Summary.Chilly,
            >= -28 and < -14 => ValueObject.Summary.Cool,
            >= -14 and < 0 => ValueObject.Summary.Mild,
            >= 0 and < 14 => ValueObject.Summary.Warm,
            >= 14 and < 28 => ValueObject.Summary.Balmy,
            >= 28 and < 42 => ValueObject.Summary.Hot,
            >= 42 and < 58 => ValueObject.Summary.Sweltering,
            > 58 => ValueObject.Summary.Scorching,
            _ => null
        };
    }

    internal static WeatherForecast Create(DateOnly date, Temperature temperature,
        MeteorologistId? meteorologistId = null) =>
        new(date, temperature, meteorologistId == Guid.Empty ? null : meteorologistId);
}