using System.Net;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Common.Authorization;
using Weather.Forecast.Feature.Forecast.Domain;
using Weather.Forecast.Feature.Forecast.Endpoint;
using Weather.Forecast.Feature.Forecast.Endpoint.Response;
using Weather.Forecast.Integration.Test.Common;

namespace Weather.Forecast.Integration.Test.Forecasts;

public class CreateRandomForecastTests(ApiFactory apiFactory) : BaseIntegrationTest(apiFactory)
{
    [Fact]
    public async Task CreateForecast_WhenIsNotAuthenticate_ShouldNotAuthorize()
    {
        //Arrange
        App.Client.DefaultRequestHeaders
            .Add(MockAuthenticationHandler.Connected, MockAuthenticationDefaults.NotConnected);

        //Act
        var (response, _) = await App.Client.POSTAsync<CreateRandomForecast, ForecastResponse>();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var context = App.CreateDbContext();

        var forecast = await context.Set<WeatherForecast>().AnyAsync();
        forecast.Should().BeFalse();
    }

    [Fact]
    public async Task CreateForecast_WhenDoesNotHaveRequiredPermission_ShouldNotAuthorize()
    {
        //Act
        var (response, _) = await App.Client.POSTAsync<CreateRandomForecast, ForecastResponse>();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        var context = App.CreateDbContext();

        var forecast = await context.Set<WeatherForecast>().AnyAsync();
        forecast.Should().BeFalse();
    }
    
    [Fact]
    public async Task CreateForecast_WhenSuccess_ShouldHaveAWeatherForecast()
    {
        //Arrange
        App.Client.DefaultRequestHeaders
            .Add(MockAuthenticationHandler.UserRole, ApplicationRole.Admin);
        
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