using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Weather.Notification.Common.Cache;
using Weather.Notification.Common.Email;

namespace Weather.Notification;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddNotificationModule(this WebApplicationBuilder builder,
        List<Assembly> moduleAssemblies)
    {
        moduleAssemblies.Add(typeof(ServiceCollectionExtensions).Assembly);

        builder.AddCacheService();
        builder.AddEmailService();

        return builder;
    }
}