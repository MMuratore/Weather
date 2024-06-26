using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.AspNetCore;
using Weather.SharedKernel.Outbox;
using Weather.SharedKernel.Persistence;

namespace Weather.SharedKernel;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddMediatR(this WebApplicationBuilder builder,
        List<Assembly> assemblies)
    {
        builder.Services.AddMediatR(o =>
        {
            o.RegisterServicesFromAssemblies(assemblies.ToArray());
            o.NotificationPublisher = new CustomTaskWhenAllPublisher();
        });

        builder.Services.AddScoped<PublishDomainEventsInterceptor>();

        return builder;
    }

    public static WebApplicationBuilder AddTransactionalDispatcher<TDbContext>(this WebApplicationBuilder builder,
        string? connectionString = default)
        where TDbContext : TransactionalDbContext
    {
        var section = builder.Configuration.GetRequiredSection(OutboxMessageProcessorOptions.Section);
        builder.Services.Configure<OutboxMessageProcessorOptions>(section);
        var options = new OutboxMessageProcessorOptions();
        section.Bind(options);

        builder.Services.AddQuartz(o =>
        {
            var jobKey = new JobKey(nameof(OutboxMessageProcessor<TDbContext>));

            o.AddJob<OutboxMessageProcessor<TDbContext>>(jobKey)
                .AddTrigger(t =>
                    t.WithIdentity(nameof(OutboxMessageProcessor<TDbContext>)).ForJob(jobKey)
                        .WithSimpleSchedule(s => s.WithInterval(options.Period).RepeatForever())
                );

            o.UsePersistentStore(c =>
            {
                c.UsePostgres(p =>
                {
                    p.ConnectionString = connectionString ?? builder.Configuration.GetConnectionString("Default") ??
                        throw new NullReferenceException("the default connection string should not be null");
                    p.TablePrefix = "quartz.";
                });
                c.UseNewtonsoftJsonSerializer();
            });
        });

        builder.Services.AddQuartzServer(o => { o.WaitForJobsToComplete = true; });

        return builder;
    }

    public static IServiceCollection RemoveHostedService<T>(this IServiceCollection services)
        where T : class, IHostedService
    {
        // Find the service descriptor for the hosted service
        var serviceDescriptor = services.FirstOrDefault(
            d => d.ServiceType == typeof(IHostedService) && d.ImplementationType == typeof(T));

        // If the service descriptor is found, remove it
        if (serviceDescriptor != null) services.Remove(serviceDescriptor);

        return services;
    }
}