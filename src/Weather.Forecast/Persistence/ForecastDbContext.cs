using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Weather.SharedKernel.Persistence;

namespace Weather.Forecast.Persistence;

internal sealed class ForecastDbContext(DbContextOptions<ForecastDbContext> options) : DbContext(options)
{
    private readonly PublishDomainEventsInterceptor? _domainEventsInterceptor;
    private const string Schema = "forecast";
    
    public ForecastDbContext(DbContextOptions<ForecastDbContext> options,
        PublishDomainEventsInterceptor? domainEventsInterceptor) : this(options)
    {
        _domainEventsInterceptor = domainEventsInterceptor;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql();
        if (_domainEventsInterceptor is not null)
        {
            optionsBuilder.AddInterceptors(_domainEventsInterceptor);
        }
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }
}
