using FluentAssertions;
using Weather.Forecast.Common;
using Weather.Forecast.Common.Forecasts;
using Weather.Forecast.Feature.Forecast.Domain;
using Weather.Forecast.Feature.Forecast.Domain.Event;

namespace Weather.Forecast.Domain.Test.Forecasts;

public class ForecastTests : BaseUnitTest
{
    [Fact]
    public void CreateForecast_WhenConstructedSuccessfully_ShouldHaveAWeatherForecastCreatedEvent()
    {
        //Arrange
        var date = Faker.Date.PastDateOnly();
        var (temperature, summary) = Faker.TemperatureWithSummary();

        //Act
        var forecast = WeatherForecast.Create(date, temperature);

        //Assert
        forecast.Temperature.Should().Be(temperature);
        forecast.Date.Should().Be(date);
        forecast.DomainEvents.Should().HaveCount(1);
        forecast.Summary.Should().Be(summary);
        forecast.MeteorologistId.Should().BeNull();

        var domainEvent = forecast.DomainEvents.First();
        domainEvent.Should().BeOfType<WeatherForecastCreated>();
        var weatherForecastCreated = (WeatherForecastCreated)domainEvent;
        weatherForecastCreated.Summary.Should().Be(summary);
        weatherForecastCreated.MeteorologistId.Should().BeNull();
    }
}