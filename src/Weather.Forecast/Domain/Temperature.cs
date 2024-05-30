namespace Weather.Forecast.Domain;

internal sealed record Temperature(decimal Celsius)
{
    public decimal Fahrenheit => Math.Round(32 + (Celsius / 0.5556m), 2);
}
