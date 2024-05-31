using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Weather.Forecast.Features.Forecasts.Domain;
using Weather.Forecast.Features.Forecasts.Endpoint.Response;
using Weather.Forecast.Persistence;
using Weather.SharedKernel;

namespace Weather.Forecast.Features.Forecasts.Endpoint;

internal sealed class GetForecast(ForecastDbContext dbContext) : Endpoint<GetForecastRequest, ForecastResponse>
{
    public override void Configure()
    {
        Get("/forecasts/{id:guid}");
        Options(o => o.WithVersionSet(WeatherApiVersion.Name).MapToApiVersion(WeatherApiVersion.DefaultApiVersion));
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "get a forecast by id";
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
