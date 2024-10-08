﻿using System.Text.Json;
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
using Weather.SharedKernel.FastEndpoint;

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
        
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(o =>
            o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

        builder.Services.AddVersioning(o =>
        {
            o.DefaultApiVersion = DefaultApiVersionSet.DefaultApiVersion;
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.ApiVersionReader = new HeaderApiVersionReader(DefaultApiVersionSet.RequiredApiVersionHeaderName);
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
                s.ApiVersion(DefaultApiVersionSet.DefaultApiVersion);
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
            o.Errors.UseProblemDetails(
                x =>
                {
                    x.AllowDuplicateErrors = false;
                    x.IndicateErrorCode = true;
                    x.TypeValue = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1";
                    x.TitleValue = "One or more validation errors occurred.";
                    x.TitleTransformer = pd => pd.Status switch
                    {
                        404 => "Not Found",
                        _ => "One or more errors occurred!"
                    };
                    o.Endpoints.Configurator = ep => { ep.PostProcessor<ExceptionPostProcessor>(Order.After); };
                });
        });

        if (app.Environment.IsProduction()) return app;

        app.UseSwaggerGen(uiConfig: o =>
        {
            o.OAuth2Client = new OAuth2ClientSettings
            {
                ClientId = options.ClientId
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