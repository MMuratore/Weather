using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Weather.Forecast.Feature.Forecast;
using Weather.Forecast.Feature.Forecast.Domain;

namespace Weather.Forecast.Feature.Meteorologist;

internal static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddMeteorologistFeature(this WebApplicationBuilder builder)
    {
        builder.AddApiVersionSets();
        
        return builder;
    }
}