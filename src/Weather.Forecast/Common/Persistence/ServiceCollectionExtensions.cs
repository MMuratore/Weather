using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Weather.Forecast.Common.Persistence;

internal static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
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

        return builder;
    }
}