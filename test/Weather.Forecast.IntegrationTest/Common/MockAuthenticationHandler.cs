using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Weather.Forecast.Integration.Test.Common;

internal sealed class MockAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string Connected = "Connected";
    public const string UserRole = "Role";
    public const string UserId = "UserId";
    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Context.Request.Headers.TryGetValue(Connected, out var connected);

        if (connected == MockAuthenticationDefaults.NotConnected)
            return Task.FromResult(AuthenticateResult.Fail("Not connected"));

        var claims = new List<Claim>
        {
            new(ClaimTypes.GivenName, MockAuthenticationDefaults.UserFirstName),
            new(ClaimTypes.Surname, MockAuthenticationDefaults.UserLastName),
            new(ClaimTypes.Email, MockAuthenticationDefaults.UserEmail)
        };

        var IsCustomUser = Context.Request.Headers.TryGetValue(UserId, out var userId);

        claims.Add(IsCustomUser
            ? new Claim(ClaimTypes.NameIdentifier,
                $"f:c6ba4477-c8fb-4662-a451-5f45f070a1e4:{userId[0]}")
            : new Claim(ClaimTypes.NameIdentifier,
                $"f:c6ba4477-c8fb-4662-a451-5f45f070a1e4:{MockAuthenticationDefaults.UserId}"));

        var userHasRole = Context.Request.Headers.TryGetValue(UserRole, out var roles);

        if (userHasRole) AddRoleToClaims(claims, roles.OfType<string>());

        var identity = new ClaimsIdentity(claims, MockAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, MockAuthenticationDefaults.AuthenticationScheme);
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }

    private static void AddRoleToClaims(List<Claim> claims, IEnumerable<string> roles)
    {
        var role = new KeycloakRealmRoles(roles);
        claims.Add(new Claim(RoleClaimType, JsonSerializer.Serialize(role), "JSON"));
    }
    
    private const string RoleClaimType = "realm_access";
    private record KeycloakRealmRoles(IEnumerable<string> roles);
}

public static class MockAuthenticationDefaults
{
    public const string AuthenticationScheme = "MockAuthenticationScheme";
    public const string UserId = "b7790473-45d5-47e5-96ba-2bcc2dabbd9b";
    public const string UserFirstName = "pedro";
    public const string UserLastName = "cain";
    public const string UserEmail = $"{UserFirstName}.{UserLastName}@acme.com";
    public const string NotConnected = "NotConnected";
}