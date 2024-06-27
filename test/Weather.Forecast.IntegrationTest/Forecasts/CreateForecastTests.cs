using System.Net;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Features.Forecasts.Domain;
using Weather.Forecast.Features.Forecasts.Endpoint;
using Weather.Forecast.Features.Forecasts.Endpoint.Response;
using Weather.Forecast.Integration.Test.Common;

namespace Weather.Forecast.Integration.Test.Forecasts;

public class CreateForecastTests(ApiFactory apiFactory) : BaseIntegrationTest(apiFactory)
{
    [Fact]
    public async Task CreateForecast_WhenSuccess_ShouldHaveAWeatherForecast()
    {
        //Act
        var (response, result) = await App.Client.POSTAsync<CreateForecast, ForecastResponse>();

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
        var (response, result) = await App.Client.POSTAsync<CreateForecast, ForecastResponse>();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var context = App.CreateDbContext();

        var forecast = await context.Set<WeatherForecast>().FirstAsync();

        forecast.Should().NotBeNull();
        forecast.Summary.Should().Be(result.Summary);
    }
}