using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Weather.Forecast.Features.Forecasts;
using Weather.Forecast.Persistence;

namespace Weather.Forecast;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddForecastModule(this WebApplicationBuilder builder,
        List<Assembly> moduleAssemblies)
    {
        var connectionString = builder.Configuration.GetConnectionString("Forecast") ??
                               throw new ArgumentNullException(nameof(builder));

        builder.Services.AddDbContext<ForecastDbContext>(
            options =>
            {
                options.UseNpgsql(connectionString, cfg =>
                {
                    if (builder.Environment.IsProduction()) cfg.EnableRetryOnFailure();
                });

                if (!builder.Environment.IsDevelopment()) return;

                options.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
                options.EnableSensitiveDataLogging();
            }, optionsLifetime: ServiceLifetime.Singleton);

        builder.AddForecastServices();

        moduleAssemblies.Add(typeof(ServiceCollectionExtensions).Assembly);

        return builder;
    }
}