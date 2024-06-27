using Weather.SharedKernel.Exception;

namespace Weather.Forecast.Features.Forecasts.Domain;

internal sealed record Temperature(decimal Celsius)
{
    private const decimal AbsoluteZero = -273.15m;
    
    public decimal Celsius { get; init; } = Celsius > AbsoluteZero ? Celsius : throw new ConflictException(TemperatureError.TemperatureCannotBeUnderAbsoluteZero);
    
    public decimal Fahrenheit => 32m + Celsius * 9m / 5m;
}