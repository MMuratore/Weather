using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Forecast.Domain;
using Weather.Forecast.Feature.Forecast.Endpoint.Response;
using Weather.SharedKernel;

namespace Weather.Forecast.Feature.Forecast.Endpoint;

internal sealed class GetRandomForecast(ILogger<GetRandomForecast> logger, ForecastDbContext dbContext) :
    EndpointWithoutRequest<Results<Ok<List<ForecastResponse>>, ProblemDetails>>
{
    public override void Configure()
    {
        Get("/forecasts/random");
        Options(o => o.WithVersionSet(WeatherApiVersion.Name).MapToApiVersion(WeatherApiVersion.DefaultApiVersion));
        AllowAnonymous();
        Summary(s => { s.Summary = "get a random number of forecast data order by date"; });
    }

    public override async Task<Results<Ok<List<ForecastResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
        var forecasts = await dbContext.Set<WeatherForecast>()
            .OrderBy(x => Guid.NewGuid())
            .Take(Random.Shared.Next(20))
            .ToListAsync(ct);

        logger.LogInformation("Retrieve {forecastCount} forecast data : '{@forecasts}'", forecasts.Count,
            forecasts.Select(x => new { x.Summary, x.Temperature.Celsius }));

        return TypedResults.Ok(forecasts.OrderBy(x => x.Date).Select(x => x.ToResponse()).ToList());
    }
}