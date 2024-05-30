using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Weather.SharedKernel.Domain;
using Weather.SharedKernel.Outbox;

namespace Weather.SharedKernel.Persistence;

public abstract class BaseDbContext(DbContextOptions options) : DbContext(options)
{
    private readonly PublishDomainEventsInterceptor? _domainEventsInterceptor;
    
    public DbSet<OutboxMessage> OutboxIntegrationEvent { get; set; }

    protected BaseDbContext(DbContextOptions options,
        PublishDomainEventsInterceptor? domainEventsInterceptor) : this(options)
    {
        _domainEventsInterceptor = domainEventsInterceptor;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
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
    
    public async Task AddIntegrationEventAsync(IIntegrationEvent message, CancellationToken cancellationToken = default)
    {
        await OutboxIntegrationEvent.AddAsync(new OutboxMessage
        {
            Id = Guid.NewGuid(),
            CreationTime = DateTimeOffset.UtcNow,
            Type = message.GetType().AssemblyQualifiedName ?? throw new ArgumentNullException(nameof(message)),
            Content = JsonSerializer.Serialize((object)message)
        }, cancellationToken);
    }
}
