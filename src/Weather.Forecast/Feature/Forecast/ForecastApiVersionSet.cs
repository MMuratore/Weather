using FastEndpoints.AspVersioning;
using Microsoft.AspNetCore.Builder;
using Weather.SharedKernel;

namespace Weather.Forecast.Feature.Forecast;

internal class ForecastApiVersionSet : DefaultApiVersionSet
{
    public const string ForecastSet = "forecast";
}

internal static class WeatherForecastApiVersionExtensions
{
    internal static WebApplicationBuilder AddApiVersionSets(this WebApplicationBuilder builder)
    {
        VersionSets.CreateApi(ForecastApiVersionSet.ForecastSet, v => v.HasApiVersion(DefaultApiVersionSet.DefaultApiVersion));
        
        return builder;
    }
}


