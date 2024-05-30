using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Weather.SharedKernel.Persistence;

public static class ServiceCollectionExtensions
{
  public static WebApplicationBuilder AddTransactionalDispatcher(this WebApplicationBuilder builder, List<Assembly> assemblies)
  {
      builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblies(assemblies.ToArray()));
      builder.Services.AddScoped<PublishDomainEventsInterceptor>();
      
      return builder;
  }
}
