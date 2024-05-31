using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Weather.Forecast.Features.Forecasts;

internal sealed class ForecastSeedHealthCheck : IHealthCheck
{
    private volatile bool _isReady;

    public bool SeedCompleted
    {
        get => _isReady;
        set => _isReady = value;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(SeedCompleted 
            ? HealthCheckResult.Healthy("The forecast seeding task has completed.")
            : HealthCheckResult.Unhealthy("That forecast seeding task is still running."));
    }
}
