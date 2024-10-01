using Weather.Forecast.Feature.Forecast.Domain;
using Weather.Forecast.Feature.Forecast.Domain.ValueObject;
using Weather.Forecast.Feature.Meteorologist.Endpoint;

namespace Weather.Forecast.Feature.Forecast.Endpoint.Response;

internal sealed record ForecastResponse(
    string Date,
    Temperature Temperature,
    Summary? Summary,
    MeteorologistResponse? Meteorologist = null);

internal static class ForecastResponseMapper
{
    public static ForecastResponse ToResponse(
        this WeatherForecast model, MeteorologistResponse? meteorologistResponse = null) =>
        new(model.Date.ToLongDateString(),
            new Temperature(decimal.Round(model.Temperature.Celsius, 2)), model.Summary, meteorologistResponse);
}