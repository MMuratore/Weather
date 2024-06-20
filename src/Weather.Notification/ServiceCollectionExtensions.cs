using System.Net.Mail;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        var section = builder.Configuration.GetSection(EmailOptions.Section);
        builder.Services.Configure<EmailOptions>(section);
        var options = new EmailOptions();
        section.Bind(options);
        
        var fluentEmailBuilder = builder.Services.AddFluentEmail(options.DefaultFrom);

        if (builder.Environment.IsDevelopment())
        {
            fluentEmailBuilder.AddSmtpSender(new SmtpClient(options.Smtp.Host, options.Smtp.Port));
        }
        
        return builder;
    }
}