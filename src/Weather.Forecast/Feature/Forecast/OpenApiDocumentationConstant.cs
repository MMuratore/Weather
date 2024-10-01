using Weather.Forecast.Feature.Forecast.Domain.ValueObject;
using Weather.Forecast.Feature.Forecast.Endpoint;
using Weather.Forecast.Feature.Forecast.Endpoint.Response;
using Weather.Forecast.Feature.Meteorologist;

namespace Weather.Forecast.Feature.Forecast;

internal static class ForecastOpenApiDocumentationConstant
{
    private const string City = "Mons";
    private const decimal Celsius = 26.6m;
    private const string Date = "Wednesday, May 16, 2001";
    
    public static readonly CreateForecastRequest CreateForecastRequest = new(Celsius);
    
    public static readonly ForecastResponse ForecastResponse = new(Date, new Temperature(Celsius), Summary.Bracing, MeteorologistOpenApiDocumentationConstant.MeteorologistResponse);
    
    public static readonly List<ForecastResponse> ForecastResponses = [ ForecastResponse, ForecastResponse ];

    public static readonly GetForecastRequest GetForecastRequest = new GetForecastRequest
    {
        Id = Guid.NewGuid()
    };
    
    public static readonly GetForecastFromRequest GetForecastFromRequest = new()
    {
        City = City
    };
}