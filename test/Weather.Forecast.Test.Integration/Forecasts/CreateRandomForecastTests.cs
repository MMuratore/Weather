using System.Net;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Feature.Forecast.Domain;
using Weather.Forecast.Feature.Forecast.Endpoint;
using Weather.Forecast.Feature.Forecast.Endpoint.Response;
using Weather.Forecast.Test.Integration.Common;

namespace Weather.Forecast.Test.Integration.Forecasts;

public class CreateRandomForecastTests(ApiFactory apiFactory) : BaseIntegrationTest(apiFactory)
{
    [Fact]
    public async Task CreateForecast_WhenSuccess_ShouldHaveAWeatherForecast()
    {
        //Act
        var (response, result) = await App.Client.POSTAsync<CreateRandomForecast, ForecastResponse>();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var context = App.CreateDbContext();

        var forecast = await context.Set<WeatherForecast>().FirstAsync();

        forecast.Should().NotBeNull();
        forecast.Summary.Should().Be(result.Summary);
    }

    [Fact]
    public async Task CreateForecast_WhenSuccess_ShouldHaveAnotherWeatherForecast()
    {
        //Act
        var (response, result) = await App.Client.POSTAsync<CreateRandomForecast, ForecastResponse>();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var context = App.CreateDbContext();

        var forecast = await context.Set<WeatherForecast>().FirstAsync();

        forecast.Should().NotBeNull();
        forecast.Summary.Should().Be(result.Summary);
    }
}