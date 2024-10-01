using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
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

        builder.AddHttpClient();
        
        return builder;
    }
    
    private static WebApplicationBuilder AddHttpClient(this WebApplicationBuilder builder)
    {
        var section = builder.Configuration.GetSection(OpenWeatherMapOptions.Section);
        builder.Services.Configure<OpenWeatherMapOptions>(section);
        var options = new OpenWeatherMapOptions();
        section.Bind(options);

        builder.Services.AddHttpClient<OpenWeatherMapClient>(client =>
        {
            client.BaseAddress = new Uri(options.BaseUrl);
        }).AddStandardResilienceHandler();

        return builder;
    }
}