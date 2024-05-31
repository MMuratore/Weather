﻿using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Weather.SharedKernel.Domain;
using Weather.SharedKernel.Outbox;

namespace Weather.SharedKernel.Persistence;

public abstract class TransactionalDbContext : BaseDbContext
{
    protected DbSet<OutboxMessage> OutboxIntegrationEvent { get; set; }
    
    protected TransactionalDbContext(DbContextOptions options,
        PublishDomainEventsInterceptor? domainEventsInterceptor) : base(options, domainEventsInterceptor)
    {
    }
    
    protected TransactionalDbContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
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