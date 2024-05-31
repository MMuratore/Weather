namespace Weather.Api.Configuration.Localization;

internal static class ConfigureLocalization
{
    private const string DefaultCulture = "en-US";
    private static readonly string[] _supportedCultures = [DefaultCulture, "fr", "nl-BE", "de-DE", "ja-JP"];
    
    internal static WebApplicationBuilder AddLocalization(this WebApplicationBuilder builder)
    {
        builder.Services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
        
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SetDefaultCulture(DefaultCulture);
            options.AddSupportedCultures(_supportedCultures);
            options.AddSupportedUICultures(_supportedCultures);
            options.ApplyCurrentCultureToResponseHeaders = true;
        });
        
        return builder;
    }
}