using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Weather.Forecast.Common.HttpClient;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Forecast;
using Weather.SharedKernel;

namespace Weather.Forecast;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddForecastModule(this WebApplicationBuilder builder,
        List<Assembly> moduleAssemblies)
    {
        moduleAssemblies.Add(typeof(ServiceCollectionExtensions).Assembly);

        builder.AddHttpClient();
        builder.AddPersistence();
        builder.AddTransactionalDispatcher<ForecastDbContext>();

        builder.AddForecastServices();

        return builder;
    }
}