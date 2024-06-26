using System.Security.Claims;
using System.Text.Encodings.Web;
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
    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.GivenName, MockAuthenticationDefaults.UserFirstName),
            new(ClaimTypes.Surname, MockAuthenticationDefaults.UserLastName),
            new(ClaimTypes.Email, MockAuthenticationDefaults.UserEmail)
        };
        
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, MockAuthenticationDefaults.AuthenticationScheme);
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}

public static class MockAuthenticationDefaults
{
    public const string AuthenticationScheme = "MockAuthenticationScheme";
    public const string UserFirstName = "firstname";
    public const string UserLastName = "lastname";
    public const string UserEmail = $"{UserFirstName}.{UserLastName}@example.org";
}