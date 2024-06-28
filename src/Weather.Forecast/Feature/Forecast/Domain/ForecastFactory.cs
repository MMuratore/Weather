using Weather.Forecast.Feature.Forecast.Domain.ValueObject;
using Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;

namespace Weather.Forecast.Feature.Forecast.Domain;

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

            var forecast = WeatherForecast.Create(_dateFrom.AddDays(Random.Shared.Next(DateRange)),
                new Temperature(celsius), meteorologistId);

            yield return forecast;
        }
    }
}