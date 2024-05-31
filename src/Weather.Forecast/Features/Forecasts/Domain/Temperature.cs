namespace Weather.Forecast.Features.Forecasts.Domain;

internal sealed record Temperature(decimal Celsius)
{
    public decimal Fahrenheit => 32m + Celsius * 9m / 5m;
}