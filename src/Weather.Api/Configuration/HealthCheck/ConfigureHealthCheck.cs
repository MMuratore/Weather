using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Weather.Api.Configuration.HealthCheck;

internal static class ConfigureHealthCheck
{
    private const string LivenessProbeName = "self";
    private const string LivenessProbeUrl = "/health/liveness";
    private const string ReadinessProbeUrl = "/health/readiness";
    
    internal static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
          .AddCheck(LivenessProbeName, () => HealthCheckResult.Healthy());
      
        return builder;
    }
    
    internal static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks(ReadinessProbeUrl, new HealthCheckOptions
            {
                Predicate = r => !r.Name.Contains(LivenessProbeName),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            }).ShortCircuit();
            
            endpoints.MapHealthChecks(LivenessProbeUrl, new HealthCheckOptions 
            {        
                Predicate = r => r.Name.Contains(LivenessProbeName)
            }).ShortCircuit();
        });
        
        return app;
    }
}
