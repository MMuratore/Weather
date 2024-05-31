using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using FastEndpoints;
using FastEndpoints.AspVersioning;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Mvc;
using Weather.SharedKernel;

namespace Weather.Api.Configuration.FastEndpoint;

internal static class ConfigureFastEndpoint
{
    private const string DefaultRoutePrefix = "api";
    
    internal static WebApplicationBuilder AddFastEndpoint(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
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
        
        builder.Services.SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.DocumentName = "v1";
                s.Title = "Weather API";
                s.ApiVersion(WeatherApiVersion.DefaultApiVersion);
            };
            o.AutoTagPathSegmentIndex = 0;
        });
        
        return builder;
    }
    
    internal static IApplicationBuilder UseFastEndpoint(this WebApplication app)
    {
        app.UseAuthentication().UseAuthorization();
        
        app.UseFastEndpoints(o =>
        {
            o.Endpoints.RoutePrefix = DefaultRoutePrefix;
            o.Errors.UseProblemDetails();
        });
        
        if (app.Environment.IsProduction()) return app;
        
        app.UseSwaggerGen();
        
        app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });
        
        return app;
    }
}