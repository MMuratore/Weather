﻿using System.Net;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Features.Meteorologists.Domain;
using Weather.Forecast.Features.Meteorologists.Endpoint;
using Weather.Forecast.Integration.Test.Common;

namespace Weather.Forecast.Integration.Test.Meteorologists;

public class CreateMeteorologistIntegrationTests(ApiFactory apiFactory) : BaseIntegrationTest(apiFactory)
{
    [Fact]
    public async Task CreateMeteorologist_WhenSuccess_ShouldHaveAWeatherForecastCreatedEvent()
    {
        //Act
        var (response, result) = await App.Client.POSTAsync<CreateMeteorologistIntegration, MeteorologistResponse>();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var context = App.CreateDbContext();

        var meteorologist = await context.Set<Meteorologist>().FirstAsync();

        meteorologist.Should().NotBeNull();
        meteorologist.Name.Fullname.Should().Be(result.Fullname);
    }
}