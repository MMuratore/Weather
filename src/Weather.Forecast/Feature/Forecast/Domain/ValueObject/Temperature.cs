using Weather.Forecast.Feature.Forecast.Domain.Error;
using Weather.SharedKernel.Domain.Exception;

namespace Weather.Forecast.Feature.Forecast.Domain.ValueObject;

internal sealed record Temperature(decimal Celsius)
{
    private const decimal AbsoluteZero = -273.15m;

    public decimal Celsius { get; init; } = Celsius > AbsoluteZero
        ? Celsius
        : throw new DomainConflictException(nameof(Celsius),TemperatureError.TemperatureCannotBeUnderAbsoluteZero);

    public decimal Fahrenheit => 32m + Celsius * 9m / 5m;
}