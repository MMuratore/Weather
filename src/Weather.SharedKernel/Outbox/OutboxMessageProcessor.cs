using System.Reflection;
using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Weather.SharedKernel.Domain;
using Weather.SharedKernel.Persistence;

namespace Weather.SharedKernel.Outbox;

public sealed class OutboxMessageProcessor<TDbContext> : BackgroundService where TDbContext : BaseDbContext
{
    private readonly ILogger<OutboxMessageProcessor<TDbContext>> _logger;
    private readonly IServiceProvider _serviceProvider;

    public OutboxMessageProcessor(ILogger<OutboxMessageProcessor<TDbContext>> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        using PeriodicTimer timer = new(TimeSpan.FromSeconds(10));
        
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWorkAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
        }
    }
    
    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
        
        var messages = await dbContext.OutboxIntegrationEvent.Where(x => x.CompleteTime == null).Take(10).ToListAsync(cancellationToken: stoppingToken);
        
        foreach (var message in messages)
        {
            try
            {
                var type = Type.GetType(message.Type);
                if (type is null)
                {
                    _logger.LogError("Type not found. Integration Event Id: '{IntegrationEventId}'", message.Id);
                    continue;
                }
                
                var integrationEvent = JsonSerializer.Deserialize(message.Content, type);
                
                if (integrationEvent == null)
                {
                    _logger.LogError("An error occurred during deserialization. Integration Event Id: '{IntegrationEventId}'", message.Id);
                    continue;
                }
                
                await publisher.Publish(integrationEvent, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred processing domain event messages.");
                message.Exception = ex.Message;
            }
            finally
            {
                message.CompleteTime = DateTime.UtcNow;
            }
        }
        
        await dbContext.SaveChangesAsync(stoppingToken);
    }
}
