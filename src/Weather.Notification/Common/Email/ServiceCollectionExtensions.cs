using System.Net.Mail;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Weather.Notification.Common.Email;

internal static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddEmailService(this WebApplicationBuilder builder)
    {
        var section = builder.Configuration.GetSection(EmailOptions.Section);
        builder.Services.Configure<EmailOptions>(section);
        var options = new EmailOptions();
        section.Bind(options);

        var fluentEmailBuilder = builder.Services.AddFluentEmail(options.DefaultFrom);

        if (builder.Environment.IsDevelopment())
            fluentEmailBuilder.AddSmtpSender(new SmtpClient(options.Smtp.Host, options.Smtp.Port));

        return builder;
    }
}