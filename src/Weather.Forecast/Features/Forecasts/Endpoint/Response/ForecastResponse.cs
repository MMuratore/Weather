using Weather.Forecast.Features.Forecasts.Domain;
using Weather.Forecast.Features.Meteorologists.Endpoint;

namespace Weather.Forecast.Features.Forecasts.Endpoint.Response;

internal sealed record ForecastResponse(
    string Date,
    Temperature Temperature,
    Summary? Summary,
    MeteorologistResponse? Meteorologist = null)
{
    /// <example>Wednesday, May 16, 2001</example>
    public string Date { get; init; } = Date;
    
    public Temperature Temperature { get; init; } = Temperature;
    public Summary? Summary { get; init; } = Summary;
    public MeteorologistResponse? Meteorologist { get; init; } = Meteorologist;
}

internal static class ForecastResponseMapper
{
    public static ForecastResponse ToResponse(
        this WeatherForecast model, MeteorologistResponse? meteorologistResponse = null) =>
        new(model.Date.ToLongDateString(),
            new Temperature(decimal.Round(model.Temperature.Celsius, 2)), model.Summary, meteorologistResponse);
}