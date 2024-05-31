namespace Weather.Forecast.Features.Forecasts.Domain;

internal readonly record struct WeatherForecastId
{
    private readonly Guid _value;
    
    private WeatherForecastId(Guid value) => _value = value;
    
    public static WeatherForecastId Empty => new(Guid.Empty);
    public static WeatherForecastId NewWeatherForecastId => new(Guid.NewGuid());
    
    public static implicit operator WeatherForecastId(Guid d) => new(d);
    
    public static explicit operator Guid(WeatherForecastId d) => d._value;
}