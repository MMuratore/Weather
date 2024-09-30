using FluentAssertions;
using Weather.Forecast.Feature.Forecast.Domain.ValueObject;
using Weather.Forecast.Test.Common;
using Weather.Forecast.Test.Common.Forecasts;

namespace Weather.Forecast.Test.Domain.Forecasts;

public class TemperatureTest : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(ListSummary))]
    public void CreateTemperature_WhenConstructedSuccessfully_ShouldBeValid(string value)
    {
        //Arrange
        var summary = Enum.Parse<Summary>(value);
        var celsius = Faker.TemperatureFromSummary(summary);
        
        //Act
        var temperature = new Temperature(celsius);

        //Assert
        temperature.Celsius.Should().Be(celsius);
    }

    public static TheoryData<string> ListSummary()
    {
        TheoryData<string> theoryData = [];

        Enum.GetNames<Summary>().ToList().ForEach(theoryData.Add);

        return theoryData;
    }
}