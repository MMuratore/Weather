using Microsoft.EntityFrameworkCore;

namespace Weather.SharedKernel.Persistence;

public abstract class BaseDbContext(DbContextOptions options) : DbContext(options)
{
    private readonly PublishDomainEventsInterceptor? _domainEventsInterceptor;
    
    protected BaseDbContext(DbContextOptions options,
        PublishDomainEventsInterceptor? domainEventsInterceptor) : this(options)
    {
        _domainEventsInterceptor = domainEventsInterceptor;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
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
