using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Weather.SharedKernel.Persistence;

namespace Weather.SharedKernel.Outbox;

public sealed class OutboxMessageProcessor<TDbContext>(
    ILogger<OutboxMessageProcessor<TDbContext>> logger,
    IServiceProvider serviceProvider, IOptions<OutboxMessageProcessorOptions> options)
    : BackgroundService
    where TDbContext : BaseDbContext
{
    private OutboxMessageProcessorOptions Options { get; } = options.Value;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{DbContextName} Outbox Message Processor Hosted Service running.", typeof(TDbContext).Name);

        using PeriodicTimer timer = new(Options.Period);
        
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await PublishIntegrationEventAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("{DbContextName} Outbox Message Processor Hosted Service is stopping.", typeof(TDbContext).Name);
        }
    }
    
    private async Task PublishIntegrationEventAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
        
        var messages = await dbContext.Set<OutboxMessage>().Where(x => x.CompleteTime == null).OrderBy(x => x.CreationTime).Take(Options.MaximumConcurrentMessage).ToListAsync(cancellationToken: stoppingToken);
        
        foreach (var message in messages)
        {
            try
            {
                var type = Type.GetType(message.Type);
                if (type is null)
                {
                    logger.LogError("Type not found. Integration Event Id: '{IntegrationEventId}'", message.Id);
                    continue;
                }
                
                var integrationEvent = JsonSerializer.Deserialize(message.Content, type);
                
                if (integrationEvent == null)
                {
                    logger.LogError("An error occurred during deserialization. Integration Event Id: '{IntegrationEventId}'", message.Id);
                    continue;
                }
                
                await publisher.Publish(integrationEvent, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred processing domain event messages.");
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
