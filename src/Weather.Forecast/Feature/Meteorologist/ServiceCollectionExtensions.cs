using Microsoft.AspNetCore.Builder;

namespace Weather.Forecast.Feature.Meteorologist;

internal static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddMeteorologistFeature(this WebApplicationBuilder builder)
    {
        builder.AddApiVersionSets();
        
        return builder;
    }
}