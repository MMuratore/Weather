using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        string connectionStringName = "Default")
        where TDbContext : TransactionalDbContext
    {
        var section = builder.Configuration.GetRequiredSection(OutboxMessageProcessorOptions.Section);
        builder.Services.Configure<OutboxMessageProcessorOptions>(section);
        var options = new OutboxMessageProcessorOptions();
        section.Bind(options);

        var connectionString = builder.Configuration.GetConnectionString(connectionStringName) ??
                               throw new NullReferenceException(
                                   $"the {connectionStringName.ToLower()} connection string should not be null");

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
                    p.ConnectionString = connectionString;
                    p.TablePrefix = "quartz.";
                });
                c.UseNewtonsoftJsonSerializer();
            });
        });

        builder.Services.AddQuartzServer(o => { o.WaitForJobsToComplete = true; });

        return builder;
    }
}