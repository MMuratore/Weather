using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Weather.Forecast.Common.HttpClient;

internal static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddHttpClient(this WebApplicationBuilder builder)
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