using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using FastEndpoints;
using FastEndpoints.AspVersioning;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.AspNetCore;
using Weather.SharedKernel;

namespace Weather.Api.Configuration.FastEndpoint;

internal static class ConfigureFastEndpoint
{
    private const string DefaultRoutePrefix = "api";
    
    internal static WebApplicationBuilder AddFastEndpoint(this WebApplicationBuilder builder)
    {
        builder.Services.AddFastEndpoints();
        
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        
        VersionSets.CreateApi(WeatherApiVersion.Name, v => v.HasApiVersion(WeatherApiVersion.DefaultApiVersion));
        
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(o =>
            o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
        
        builder.Services.AddVersioning(o =>
        {
            o.DefaultApiVersion = WeatherApiVersion.DefaultApiVersion;
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.ApiVersionReader = new HeaderApiVersionReader(WeatherApiVersion.RequiredApiVersionHeaderName);
        });
        
        var options = new SwaggerOAuthOptions();
        var section = builder.Configuration.GetSection(SwaggerOAuthOptions.Section);
        section.Bind(options);
        builder.Services.Configure<SwaggerOAuthOptions>(section);
        
        builder.Services.SwaggerDocument(o =>
        {
            o.EnableJWTBearerAuth = false;
            o.AutoTagPathSegmentIndex = 0;
            
            o.DocumentSettings = s =>
            {
                s.DocumentName = "v1";
                s.Title = "Weather API";
                s.ApiVersion(WeatherApiVersion.DefaultApiVersion);
                s.AddAuth("oidc", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = options.AuthorizationUrl,
                            TokenUrl = options.TokenUrl,
                            RefreshUrl = options.TokenUrl,
                            Scopes = options.Scopes.ToDictionary(x => x, x => x)
                        }
                    }
                });
            };
        });
        
        return builder;
    }
    
    internal static IApplicationBuilder UseFastEndpoint(this WebApplication app)
    {
        var options = app.Services.GetRequiredService<IOptions<SwaggerOAuthOptions>>().Value;
       
        app.UseFastEndpoints(o =>
        {
            o.Endpoints.RoutePrefix = DefaultRoutePrefix;
            o.Errors.UseProblemDetails();
        });
        
        if (app.Environment.IsProduction()) return app;
        
        app.UseSwaggerGen(uiConfig: o =>
        {
            o.OAuth2Client = new OAuth2ClientSettings
            {
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret
            };
        });
        
        app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });
        
        return app;
    }
}