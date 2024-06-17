using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Weather.Forecast.Features.Forecasts.Client;
using Weather.Forecast.Features.Forecasts.Endpoint.Response;
using Weather.SharedKernel;

namespace Weather.Forecast.Features.Forecasts.Endpoint;

internal sealed class GetForecastFrom(OpenWeatherMapClient client)
    : Endpoint<GetForecastFromRequest, ForecastResponse>
{
    public override void Configure()
    {
        Get("/forecasts");
        Options(o => o.WithVersionSet(WeatherApiVersion.Name).MapToApiVersion(WeatherApiVersion.DefaultApiVersion));
        Summary(s => { s.Summary = "get forecast data from a city"; });
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