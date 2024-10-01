using FastEndpoints;
using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;
using Weather.SharedKernel;

namespace Weather.Forecast.Feature.Meteorologist.Endpoint;

internal sealed class GetMeteorologist(ForecastDbContext dbContext)
    : Endpoint<GetMeteorologistRequest, MeteorologistResponse>
{
    public override void Configure()
    {
        Get("/meteorologists/{id:guid}");
        Options(o => o.WithVersionSet(MeteorologistApiVersionSet.MeteorologistSet).MapToApiVersion(DefaultApiVersionSet.DefaultApiVersion));
        Summary(s =>
        {
            s.Summary = "get a meteorologist by id";
            s.ExampleRequest = MeteorologistOpenApiDocumentationConstant.GetMeteorologistRequest;
            s.Response(example: MeteorologistOpenApiDocumentationConstant.MeteorologistResponse);
        });
    }

    public override Task HandleAsync(GetMeteorologistRequest req, CancellationToken ct)
    {
        var meteorologist = dbContext.Set<Domain.Meteorologist>().FirstOrDefault(x => x.Id == req.Id);

        return meteorologist is null ? SendNotFoundAsync(ct) : SendOkAsync(meteorologist.ToResponse(), ct);
    }
}



internal sealed record GetMeteorologistRequest
{
    public Guid Id { get; init; }
}

internal sealed record MeteorologistResponse(string Fullname, int Age, Prestige Prestige);

internal static class MeteorologistResponseMapper
{
    public static MeteorologistResponse ToResponse(this Domain.Meteorologist model) =>
        new(model.Name.Fullname, model.BirthDay.Age, model.Prestige);
}

