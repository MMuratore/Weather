using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Weather.Forecast.Features.Forecasts;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddForecastServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ForecastSeedHealthCheck>();
        builder.Services.AddHealthChecks().AddCheck<ForecastSeedHealthCheck>(nameof(ForecastSeedHealthCheck));
        builder.Services.AddHostedService<ForecastSeederHostedService>();
        builder.Services.AddSingleton<ForecastFactory>();
        
        return builder;
    }
}