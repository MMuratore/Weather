using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace Weather.Notification;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddNotificationModule(this WebApplicationBuilder builder,
        List<Assembly> moduleAssemblies)
    {
        moduleAssemblies.Add(typeof(ServiceCollectionExtensions).Assembly);

        return builder;
    }
}