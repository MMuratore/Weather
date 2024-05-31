using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Weather.SharedKernel.Persistence;

namespace Weather.Forecast.Persistence;

internal sealed class ForecastDbContext : TransactionalDbContext
{
    private const string Schema = "forecast";
    
    public ForecastDbContext(DbContextOptions<ForecastDbContext> options) : base(options)
    {
    }
    
    public ForecastDbContext(DbContextOptions<ForecastDbContext> options,
        PublishDomainEventsInterceptor domainEventsInterceptor) : base(options, domainEventsInterceptor)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql();
        
        base.OnConfiguring(optionsBuilder);
    }
}