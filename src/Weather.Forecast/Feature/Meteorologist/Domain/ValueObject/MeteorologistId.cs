namespace Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;

internal readonly record struct MeteorologistId
{
    private readonly Guid _value;

    private MeteorologistId(Guid value) => _value = value;

    public static MeteorologistId Empty => new(Guid.Empty);
    public static MeteorologistId NewWeatherForecastId => new(Guid.NewGuid());

    public static implicit operator MeteorologistId(Guid d) => new(d);

    public static explicit operator Guid(MeteorologistId d) => d._value;
}