using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Weather.Forecast.Feature.Forecast.Domain;

namespace Weather.Forecast.Feature.Forecast;

internal static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddForecastFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ForecastSeedHealthCheck>();
        builder.Services.AddHealthChecks().AddCheck<ForecastSeedHealthCheck>(nameof(ForecastSeedHealthCheck));

        builder.Services.AddHostedService<ForecastSeederHostedService>();
        builder.Services.AddSingleton<ForecastFactory>();

        builder.AddApiVersionSets();
        
        return builder;
    }
}