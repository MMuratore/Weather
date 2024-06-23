using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Weather.SharedKernel.Persistence;

namespace Weather.SharedKernel.Outbox;

[DisallowConcurrentExecution]
public sealed class OutboxMessageProcessor<TDbContext>(
    ILogger<OutboxMessageProcessor<TDbContext>> logger,
    IPublisher publisher,
    TDbContext dbContext,
    IOptions<OutboxMessageProcessorOptions> options)
    : IJob where TDbContext : TransactionalDbContext
{
    private OutboxMessageProcessorOptions Options { get; } = options.Value;

    public async Task Execute(IJobExecutionContext context) =>
        await ProcessOutboxMessageAsync(context.CancellationToken);

    private async Task ProcessOutboxMessageAsync(CancellationToken stoppingToken)
    {
        var messages = await GetOutboxMessages(stoppingToken);

        foreach (var message in messages)
        {
            await PublishIntegrationEventAsync(message, stoppingToken);
        }
    }

    private Task<List<OutboxMessage>> GetOutboxMessages(CancellationToken stoppingToken)
    {
        var query = dbContext.Set<OutboxMessage>().Where(x => x.CompleteTime == null)
            .OrderBy(x => x.CreationTime).AsQueryable();

        if (Options.MaximumConcurrentMessage is not null) query = query.Take(Options.MaximumConcurrentMessage.Value);

        return query.AsNoTracking().ToListAsync(stoppingToken);
    }
    
    private async Task PublishIntegrationEventAsync(OutboxMessage message, CancellationToken stoppingToken)
    {
        var type = Type.GetType(message.Type);
        if (type is null)
        {
            logger.LogError("Type not found. Integration Event Id: '{IntegrationEventId}'", message.Id);
            message.UncaughtExceptions.Add("Type not found.");
            return;
        }

        var integrationEvent = JsonSerializer.Deserialize(message.Content, type);
        
        if (integrationEvent == null)
        {
            logger.LogError(
                "An error occurred during deserialization. Integration Event Id: '{IntegrationEventId}'",
                message.Id);
            message.UncaughtExceptions.Add("An error occurred during deserialization.");
            return;
        }

        try
        {
            await publisher.Publish(integrationEvent, stoppingToken);
        }
        catch (AggregateException e)
        {
            foreach (var innerException in e.InnerExceptions)
            {
                logger.LogError(innerException,
                    "An error occurred during execution. Integration Event Id: '{IntegrationEventId}'",
                    message.Id);
                message.UncaughtExceptions.Add(innerException.Message);
            }
        }
        
        await UpdateOutboxMessages(message, stoppingToken);
    }

    private async Task UpdateOutboxMessages(OutboxMessage message, CancellationToken stoppingToken)
    {
        if (message.UncaughtExceptions.Count == 0)
            await dbContext.OutboxIntegrationEvent.Where(x => x.Id == message.Id).ExecuteDeleteAsync(stoppingToken);
        else
            await dbContext.OutboxIntegrationEvent.Where(x => x.Id == message.Id).ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(b => b.CompleteTime, DateTime.UtcNow)
                    .SetProperty(b => b.UncaughtExceptions, message.UncaughtExceptions), stoppingToken);
    }
}