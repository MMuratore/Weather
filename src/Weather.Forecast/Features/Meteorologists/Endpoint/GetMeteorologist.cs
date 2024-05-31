using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Weather.Forecast.Features.Meteorologists.Domain;
using Weather.Forecast.Persistence;
using Weather.SharedKernel;

namespace Weather.Forecast.Features.Meteorologists.Endpoint;

internal sealed class GetMeteorologist(ForecastDbContext dbContext)
    : Endpoint<GetMeteorologistRequest, MeteorologistResponse>
{
    public override void Configure()
    {
        Get("/meteorologists/{id:guid}");
        Options(o => o.WithVersionSet(WeatherApiVersion.Name).MapToApiVersion(WeatherApiVersion.DefaultApiVersion));
        AllowAnonymous();
        Summary(s => { s.Summary = "get a meteorologist by id"; });
    }
    
    public override Task HandleAsync(GetMeteorologistRequest req, CancellationToken ct)
    {
        var meteorologist = dbContext.Set<Meteorologist>().FirstOrDefault(x => x.Id == req.Id);
        
        return meteorologist is null ? SendNotFoundAsync(ct) : SendOkAsync(meteorologist.ToResponse(), ct);
    }
}

internal sealed record MeteorologistResponse(string Fullname, int Age, Prestige Prestige);

internal static class MeteorologistResponseMapper
{
    public static MeteorologistResponse ToResponse(this Meteorologist model) =>
        new(model.Name.Fullname, model.BirthDay.Age, model.Prestige);
}

internal sealed record GetMeteorologistRequest
{
    public Guid Id { get; init; }
}