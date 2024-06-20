using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Weather.Notification;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddNotificationModule(this WebApplicationBuilder builder,
        List<Assembly> moduleAssemblies)
    {
        moduleAssemblies.Add(typeof(ServiceCollectionExtensions).Assembly);

        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<InMemoryMeteorologistCache>();
        builder.Services.AddScoped<IMeteorologistCache, ReadThroughMeteorologistCache>();

        return builder;
    }
}