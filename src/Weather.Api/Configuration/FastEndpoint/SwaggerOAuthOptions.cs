namespace Weather.Api.Configuration.FastEndpoint;

internal sealed class SwaggerOAuthOptions
{
    public const string Section = "Authentication:Swagger";

    public string ClientId { get; set; } = string.Empty;
    public string AuthorizationUrl { get; set; } = string.Empty;
    public string TokenUrl { get; set; } = string.Empty;
    public IEnumerable<string> Scopes { get; set; } = [];
}