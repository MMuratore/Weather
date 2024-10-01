using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Weather.Forecast.Common.Authorization;

internal static class ServiceCollectionExtensions
{
    internal static WebApplicationBuilder AddForecastAuthorizationPolicy(this WebApplicationBuilder builder)
    {
            builder.Services.AddAuthorizationBuilder().AddPolicy(
                ApplicationAuthorizationPolicy.Admin,
                policy =>
                    policy.RequireRealmRoles(ApplicationRole.Admin)
            );
        
            return builder;
    }
}