using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Weather.Forecast.Common.Authorization;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Forecast;
using Weather.Forecast.Feature.Meteorologist;
using Weather.SharedKernel;

namespace Weather.Forecast;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddForecastModule(this WebApplicationBuilder builder,
        List<Assembly> moduleAssemblies)
    {
        moduleAssemblies.Add(typeof(ServiceCollectionExtensions).Assembly);
        
        builder.AddPersistence();
        builder.AddTransactionalDispatcher<ForecastDbContext>();
        builder.AddForecastAuthorizationPolicy();
        
        builder.AddForecastFeature();
        builder.AddMeteorologistFeature();
        
        return builder;
    }
}