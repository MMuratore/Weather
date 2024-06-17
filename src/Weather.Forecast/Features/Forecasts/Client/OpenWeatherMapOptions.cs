namespace Weather.Forecast.Features.Forecasts.Client;

internal class OpenWeatherMapOptions
{
    public const string Section = "OpenWeatherMap";

    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}