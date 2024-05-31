using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weather.SharedKernel.Outbox;

namespace Weather.SharedKernel.Persistence;

public static class ServiceCollectionExtensions
{
  public static WebApplicationBuilder AddTransactionalDispatcher(this WebApplicationBuilder builder, List<Assembly> assemblies)
  {
      builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblies(assemblies.ToArray()));
      builder.Services.AddScoped<PublishDomainEventsInterceptor>();
      builder.Services.Configure<OutboxMessageProcessorOptions>(builder.Configuration.GetRequiredSection(OutboxMessageProcessorOptions.Section));
      
      return builder;
  }
}
