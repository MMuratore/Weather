﻿using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Weather.Forecast.Feature.Forecast.Endpoint.Response;
using Weather.SharedKernel;

namespace Weather.Forecast.Feature.Forecast.Endpoint;

internal sealed class GetForecastFrom(OpenWeatherMapClient client)
    : Endpoint<GetForecastFromRequest, ForecastResponse>
{
    public override void Configure()
    {
        Get("/forecasts");
        Options(o => o.WithVersionSet(ForecastApiVersionSet.ForecastSet).MapToApiVersion(DefaultApiVersionSet.DefaultApiVersion));
        Summary(s =>
        {
            s.Summary = "get forecast data from a city"; 
            s.ExampleRequest = ForecastOpenApiDocumentationConstant.GetForecastFromRequest;
            s.Response(example: ForecastOpenApiDocumentationConstant.ForecastResponse);
        });
    }

    public override async Task HandleAsync(GetForecastFromRequest req, CancellationToken ct)
    {
        var forecast = await client.GetForecastFrom(req.City);
        
        if (forecast is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        await SendOkAsync(forecast.ToResponse(), ct);
    }
}

internal sealed record GetForecastFromRequest
{
    [QueryParam] public required string City { get; init; }
}