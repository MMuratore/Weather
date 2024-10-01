using System.Net;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Feature.Meteorologist.Domain;
using Weather.Forecast.Feature.Meteorologist.Endpoint;
using Weather.Forecast.Test.Integration.Common;

namespace Weather.Forecast.Test.Integration.Meteorologists;

public class CreateMeteorologistTests(ApiFactory apiFactory) : BaseIntegrationTest(apiFactory)
{
    [Fact]
    public async Task CreateMeteorologist_WhenSuccess_ShouldHaveAWeatherForecastCreatedEvent()
    {
        //Act
        var (response, result) = await App.Client.POSTAsync<CreateMeteorologist, MeteorologistResponse>();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var context = App.CreateDbContext();

        var meteorologist = await EntityFrameworkQueryableExtensions.FirstAsync(context.Set<Meteorologist>());

        AssertionExtensions.Should((object)meteorologist).NotBeNull();
        AssertionExtensions.Should((string)meteorologist.Name.Fullname).Be(result.Fullname);
    }
}