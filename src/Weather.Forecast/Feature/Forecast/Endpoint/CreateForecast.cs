using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Forecast.Domain;
using Weather.Forecast.Feature.Forecast.Domain.ValueObject;
using Weather.Forecast.Feature.Forecast.Endpoint.Response;
using Weather.Forecast.Feature.Meteorologist.Endpoint;
using Weather.SharedKernel;

namespace Weather.Forecast.Feature.Forecast.Endpoint;

internal sealed class CreateForecast(ForecastDbContext dbContext)
    : Endpoint<CreateForecastRequest, ForecastResponse>
{
    public override void Configure()
    {
        Post("/forecasts");
        Options(o => o.WithVersionSet(ForecastApiVersionSet.ForecastSet).MapToApiVersion(DefaultApiVersionSet.DefaultApiVersion));
        Summary(s =>
        {
            s.Summary = "generate a forecast";
            s.ExampleRequest = ForecastOpenApiDocumentationConstant.CreateForecastRequest;
        });
    }

    public override async Task HandleAsync(CreateForecastRequest req, CancellationToken ct)
    {
        var meteorologist = dbContext.Set<Meteorologist.Domain.Meteorologist>().AsNoTracking()
            .OrderBy(x => Guid.NewGuid()).FirstOrDefault();

        var temperature = new Temperature(req.Celsius);
        var date = DateOnly.FromDateTime(TimeProvider.System.GetUtcNow().DateTime);
        var weatherForecast = WeatherForecast.Create(date, temperature);

        await dbContext.Set<WeatherForecast>().AddAsync(weatherForecast, ct);
        await dbContext.SaveChangesAsync(ct);

        await SendCreatedAtAsync<GetForecast>(new { Id = (Guid)weatherForecast.Id },
            weatherForecast.ToResponse(meteorologist?.ToResponse()), cancellation: ct);
    }
}

internal sealed record CreateForecastRequest(decimal Celsius);