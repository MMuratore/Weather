using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Forecast.Domain;
using Weather.Forecast.Feature.Forecast.Endpoint.Response;
using Weather.Forecast.Feature.Meteorologist.Endpoint;
using Weather.SharedKernel;

namespace Weather.Forecast.Feature.Forecast.Endpoint;

internal sealed class CreateRandomForecast(ForecastFactory factory, ForecastDbContext dbContext)
    : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/forecasts/random");
        Options(o => o.WithVersionSet(WeatherApiVersion.Name).MapToApiVersion(WeatherApiVersion.DefaultApiVersion));
        Summary(s => { s.Summary = "generate a random forecast"; });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var meteorologist = dbContext.Set<Meteorologist.Domain.Meteorologist>().AsNoTracking()
            .OrderBy(x => Guid.NewGuid()).FirstOrDefault();

        var weatherForecast = factory.Create(meteorologistId: meteorologist?.Id).First();

        await dbContext.Set<WeatherForecast>().AddAsync(weatherForecast, ct);
        await dbContext.SaveChangesAsync(ct);

        await SendCreatedAtAsync<GetForecast>(new { Id = (Guid)weatherForecast.Id },
            weatherForecast.ToResponse(meteorologist?.ToResponse()), cancellation: ct);
    }
}