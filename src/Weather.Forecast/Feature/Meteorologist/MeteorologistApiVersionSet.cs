using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Weather.SharedKernel;

namespace Weather.Forecast.Feature.Meteorologist;

internal class MeteorologistApiVersionSet : DefaultApiVersionSet
{
    public const string MeteorologistSet = "meteorologist";
}

internal static class WeatherForecastApiVersionExtensions
{
    internal static WebApplicationBuilder AddApiVersionSets(this WebApplicationBuilder builder)
    {
        VersionSets.CreateApi(MeteorologistApiVersionSet.MeteorologistSet, v => v.HasApiVersion(DefaultApiVersionSet.DefaultApiVersion));
        
        return builder;
    }
}


