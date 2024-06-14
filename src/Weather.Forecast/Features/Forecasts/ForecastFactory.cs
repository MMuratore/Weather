using Weather.Forecast.Features.Forecasts.Domain;
using Weather.Forecast.Features.Meteorologists.Domain;

namespace Weather.Forecast.Features.Forecasts;

internal sealed class ForecastFactory
{
    private const int DateRange = 365;
    private const decimal TemperatureFrom = -70m;
    private const decimal TemperatureTo = 70m;

    private readonly DateOnly _dateFrom =
        DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().Date.Add(-TimeSpan.FromDays(DateRange)));

    public IEnumerable<WeatherForecast> Create(int number = 1, MeteorologistId? meteorologistId = null)
    {
        for (var i = 0; i < number; i++)
        {
            var celsius = TemperatureFrom +
                          (decimal)(Random.Shared.NextDouble() * (double)(TemperatureTo - TemperatureFrom));

            Summary? summary = celsius switch
            {
                < -56 => Summary.Freezing,
                >= -56 and < -42 => Summary.Bracing,
                >= -42 and < -28 => Summary.Chilly,
                >= -28 and < -14 => Summary.Cool,
                >= -14 and < 0 => Summary.Mild,
                >= 0 and < 14 => Summary.Warm,
                >= 14 and < 28 => Summary.Balmy,
                >= 28 and < 42 => Summary.Hot,
                >= 42 and < 58 => Summary.Sweltering,
                > 58 => Summary.Scorching,
                _ => null
            };

            yield return WeatherForecast.Create(_dateFrom.AddDays(Random.Shared.Next(DateRange)),
                new Temperature(celsius), summary, meteorologistId);
        }
    }
}