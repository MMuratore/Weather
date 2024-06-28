using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Weather.Notification.Common.Cache;

internal static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddCacheService(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<InMemoryMeteorologistCache>();
        builder.Services.AddScoped<IMeteorologistCache, ReadThroughMeteorologistCache>();

        return builder;
    }
}