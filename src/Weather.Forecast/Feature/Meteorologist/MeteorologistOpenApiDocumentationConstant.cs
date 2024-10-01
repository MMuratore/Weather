using Weather.Forecast.Feature.Meteorologist.Domain.ValueObject;
using Weather.Forecast.Feature.Meteorologist.Endpoint;

namespace Weather.Forecast.Feature.Meteorologist;

internal static class MeteorologistOpenApiDocumentationConstant
{
    private const string Fullname = "Freddy Sipes";
    private const int Age = 45;
    private const Prestige Prestige = Domain.ValueObject.Prestige.Casual;

    public static readonly MeteorologistResponse MeteorologistResponse = new MeteorologistResponse(Fullname, Age, Prestige);
    public static GetMeteorologistRequest GetMeteorologistRequest = new GetMeteorologistRequest
    {
        Id = Guid.NewGuid()
    };
}