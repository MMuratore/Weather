using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weather.Forecast.Features.Forecasts.Client;
using Weather.Forecast.Features.Forecasts.Domain;

namespace Weather.Forecast.Features.Forecasts;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddForecastServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ForecastSeedHealthCheck>();
        builder.Services.AddHealthChecks().AddCheck<ForecastSeedHealthCheck>(nameof(ForecastSeedHealthCheck));
        builder.Services.AddHostedService<ForecastSeederHostedService>();
        builder.Services.AddSingleton<ForecastFactory>();

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