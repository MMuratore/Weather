using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.AspNetCore;
using Weather.SharedKernel.Outbox;

namespace Weather.SharedKernel.Persistence;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddTransactionalDispatcher(this WebApplicationBuilder builder,
        List<Assembly> assemblies)
    {
        builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblies(assemblies.ToArray()));
        builder.Services.AddScoped<PublishDomainEventsInterceptor>();

        var section = builder.Configuration.GetRequiredSection(OutboxMessageProcessorOptions.Section);
        builder.Services.Configure<OutboxMessageProcessorOptions>(section);
        var options = new OutboxMessageProcessorOptions();
        section.Bind(options);

        var connectionString = builder.Configuration.GetConnectionString("Default") ??
                               throw new NullReferenceException(
                                   "the default connection string should not be null");

        builder.Services.AddDbContext<TransactionalDbContext>(
            o =>
            {
                o.UseNpgsql(connectionString, cfg =>
                {
                    if (builder.Environment.IsProduction()) cfg.EnableRetryOnFailure();
                });

                if (!builder.Environment.IsDevelopment()) return;

                o.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
                o.EnableSensitiveDataLogging();
            }, optionsLifetime: ServiceLifetime.Singleton);

        builder.Services.AddQuartz(o =>
        {
            var jobKey = new JobKey(nameof(OutboxMessageProcessor));

            o.AddJob<OutboxMessageProcessor>(jobKey)
                .AddTrigger(t =>
                    t.WithIdentity(nameof(OutboxMessageProcessor)).ForJob(jobKey)
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