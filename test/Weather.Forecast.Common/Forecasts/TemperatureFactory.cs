using Bogus;
using Weather.Forecast.Common.Constants;
using Weather.Forecast.Features.Forecasts.Domain;

namespace Weather.Forecast.Common.Forecasts;

internal static class TemperatureFactory
{
    internal static Temperature Temperature(this Randomizer randomizer, decimal min = -50m, decimal max = 100m) =>
        new(randomizer.Decimal(min, max));
    
    internal static (Temperature, Summary) TemperatureWithSummary(this Faker faker,
        decimal celsius = TestConstants.Temperature.Balmy)
    {
        var summary = celsius switch
        {
            < -56 => Summary.Freezing,
            >= -56 and < -42 => Summary.Bracing,
            >= -42 and < -28 => Summary.Chilly,
            >= -28 and < -14 => Summary.Cool,
            >= -14 and < 0 => Summary.Mild,
            >= 0 and < 14 => Summary.Warm,
            >= 14 and < 28 => Summary.Balmy,
            >= 28 and < 42 => Summary.Hot,
            >= 42 and < 58 => Summary.Sweltering,
            _ => Summary.Scorching
        };
        
        return (new Temperature(celsius), summary);
    }
    
    internal static decimal TemperatureFromSummary(this Faker faker, Summary summary)
    {
        var celsius = summary switch
        {
            Summary.Freezing => -68,
            Summary.Bracing => -41,
            Summary.Chilly => -32,
            Summary.Cool => -16,
            Summary.Mild => -5,
            Summary.Warm => 10,
            Summary.Balmy => 22,
            Summary.Hot => 35,
            Summary.Sweltering => 47,
            Summary.Scorching => 61,
            _ => 100
        };
        
        return celsius;
    }
}