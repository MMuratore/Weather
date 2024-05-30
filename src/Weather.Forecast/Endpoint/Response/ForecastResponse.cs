using Weather.Forecast.Domain;

namespace Weather.Forecast.Endpoint.Response;

internal sealed record ForecastResponse(string Date, Temperature Temperature, Summary? Summary)
{
    /// <example>Wednesday, May 16, 2001</example>
    public string Date { get; init; } = Date;
    public Temperature Temperature { get; init; } = Temperature;
    public Summary? Summary { get; init; } = Summary;
}

internal static class ForecastResponseMapper
{
    public static ForecastResponse ToResponse(
        this WeatherForecast model)
    {
        return new ForecastResponse(model.Date.ToLongDateString(), model.Temperature, model.Summary);
    }
}
