using Bogus;

namespace Weather.Forecast.Test.Common;

public abstract class BaseUnitTest
{
    protected readonly Faker Faker = new();
}