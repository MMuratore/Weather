namespace Weather.Api.Configuration.Security;

internal static class ConfigureAuthentication
{
    internal static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication().AddJwtBearer();

        return builder;
    }
}