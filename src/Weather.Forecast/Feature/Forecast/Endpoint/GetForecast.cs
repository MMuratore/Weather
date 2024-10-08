﻿using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Forecast.Domain;
using Weather.Forecast.Feature.Forecast.Endpoint.Response;
using Weather.SharedKernel;

namespace Weather.Forecast.Feature.Forecast.Endpoint;

internal sealed class GetForecast(ForecastDbContext dbContext) : Endpoint<GetForecastRequest, ForecastResponse>
{
    public override void Configure()
    {
        Get("/forecasts/{id:guid}");
        Options(o => o.WithVersionSet(ForecastApiVersionSet.ForecastSet).MapToApiVersion(DefaultApiVersionSet.DefaultApiVersion));
        Summary(s =>
        {
            s.Summary = "get a forecast by id"; 
            s.ExampleRequest = ForecastOpenApiDocumentationConstant.GetForecastRequest;
            s.Response(example: ForecastOpenApiDocumentationConstant.ForecastResponse);
        });
    }

    public override Task HandleAsync(GetForecastRequest req, CancellationToken ct)
    {
        var forecast = dbContext.Set<WeatherForecast>().FirstOrDefault(x => x.Id == req.Id);

        return forecast is null ? SendNotFoundAsync(ct) : SendOkAsync(forecast.ToResponse(), ct);
    }
}

internal sealed record GetForecastRequest
{
    public Guid Id { get; init; }
}