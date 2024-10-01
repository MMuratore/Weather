using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Weather.Api.Configuration.Security;

internal static class ConfigureAuthorization
{
    internal static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddKeycloakAuthorization()
            .AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());
        
        return builder;
    }
}