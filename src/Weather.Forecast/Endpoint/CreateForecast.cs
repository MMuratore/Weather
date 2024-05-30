using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Weather.Forecast.Domain;
using Weather.Forecast.Endpoint.Response;
using Weather.Forecast.Persistence;
using Weather.SharedKernel;

namespace Weather.Forecast.Endpoint;

internal sealed class CreateForecast(ForecastFactory factory, ForecastDbContext dbContext) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/forecasts/random");
        Options(o => o.WithVersionSet(WeatherApiVersion.Name).MapToApiVersion(WeatherApiVersion.DefaultApiVersion));
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "generate a random forecast";
        });
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var weatherForecast = factory.Create().First();
        
        await dbContext.Set<WeatherForecast>().AddAsync(weatherForecast, ct);
        await dbContext.SaveChangesAsync(ct);
        
        await SendCreatedAtAsync<GetForecast>(new { Id = (Guid)weatherForecast.Id }, weatherForecast.ToResponse(), cancellation: ct);
    }
}
