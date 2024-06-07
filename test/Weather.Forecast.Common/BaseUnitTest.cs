using Bogus;

namespace Weather.Forecast.Common;

public abstract class BaseUnitTest
{
    protected readonly Faker Faker = new ();
}