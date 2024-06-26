﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Weather.Forecast.Common.Persistence;
using Weather.Forecast.Feature.Forecast.Domain;

namespace Weather.Forecast.Feature.Forecast;

internal sealed class ForecastSeederHostedService(
    ILogger<ForecastSeederHostedService> logger,
    IServiceProvider provider,
    ForecastSeedHealthCheck forecastSeedHealthCheck,
    ForecastFactory forecastFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = provider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ForecastDbContext>();
        dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        dbContext.Database.AutoSavepointsEnabled = false;
        dbContext.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

        while (dbContext.Set<WeatherForecast>().Count() < 1000000)
        {
            logger.LogInformation("Database forecast seeder start");

            for (var i = 0; i < 100; i++)
            {
                var forecasts = forecastFactory.Create(10000).ToArray();

                foreach (var forecast in forecasts) forecast.ClearDomainEvents();

                await dbContext.Set<WeatherForecast>().AddRangeAsync(forecasts, stoppingToken);
                await dbContext.SaveChangesAsync(stoppingToken);
            }

            logger.LogInformation("Database forecast seeder finished");
        }

        forecastSeedHealthCheck.SeedCompleted = true;
    }
}