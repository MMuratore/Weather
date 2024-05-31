using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Features.Forecasts.Domain;
using Weather.Forecast.Features.Forecasts.Endpoint.Response;
using Weather.Forecast.Features.Meteorologists.Domain;
using Weather.Forecast.Features.Meteorologists.Endpoint;
using Weather.Forecast.Persistence;
using Weather.SharedKernel;

namespace Weather.Forecast.Features.Forecasts.Endpoint;

internal sealed class CreateForecast(ForecastFactory factory, ForecastDbContext dbContext) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/forecasts/random");
        Options(o => o.WithVersionSet(WeatherApiVersion.Name).MapToApiVersion(WeatherApiVersion.DefaultApiVersion));
        AllowAnonymous();
        Summary(s => { s.Summary = "generate a random forecast"; });
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var meteorologist = dbContext.Set<Meteorologist>().AsNoTracking().OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        
        var weatherForecast = factory.Create(meteorologistId: meteorologist?.Id).First();
        
        await dbContext.Set<WeatherForecast>().AddAsync(weatherForecast, ct);
        await dbContext.SaveChangesAsync(ct);
        
        await SendCreatedAtAsync<GetForecast>(new { Id = (Guid)weatherForecast.Id },
            weatherForecast.ToResponse(meteorologist?.ToResponse()), cancellation: ct);
    }
}