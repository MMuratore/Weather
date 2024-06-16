using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Weather.SharedKernel.Persistence;

namespace Weather.SharedKernel.Outbox;

[DisallowConcurrentExecution]
public sealed class OutboxMessageProcessor(
    ILogger<OutboxMessageProcessor> logger,
    IPublisher publisher,
    TransactionalDbContext dbContext,
    IOptions<OutboxMessageProcessorOptions> options)
    : IJob
{
    private OutboxMessageProcessorOptions Options { get; } = options.Value;

    public async Task Execute(IJobExecutionContext context) =>
        await PublishIntegrationEventAsync(context.CancellationToken);

    private async Task PublishIntegrationEventAsync(CancellationToken stoppingToken)
    {
        var query = dbContext.Set<OutboxMessage>().Where(x => x.CompleteTime == null)
            .OrderBy(x => x.CreationTime).AsQueryable();

        if (Options.MaximumConcurrentMessage is not null) query = query.Take(Options.MaximumConcurrentMessage.Value);

        var messages = await query.AsNoTracking().ToListAsync(stoppingToken);

        foreach (var message in messages)
            try
            {
                var type = Type.GetType(message.Type);
                if (type is null)
                {
                    logger.LogError("Type not found. Integration Event Id: '{IntegrationEventId}'", message.Id);
                    message.Exception = "Type not found";
                    continue;
                }

                var integrationEvent = JsonSerializer.Deserialize(message.Content, type);

                if (integrationEvent == null)
                {
                    logger.LogError(
                        "An error occurred during deserialization. Integration Event Id: '{IntegrationEventId}'",
                        message.Id);
                    message.Exception = "An error occurred during deserialization";
                    continue;
                }

                await publisher.Publish(integrationEvent, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred processing domain event messages.");
                message.Exception = ex.Message;
            }
            finally
            {
                await dbContext.OutboxIntegrationEvent.Where(x => x.Id == message.Id).ExecuteUpdateAsync(setters =>
                    setters
                        .SetProperty(b => b.CompleteTime, DateTime.UtcNow)
                        .SetProperty(b => b.Exception, message.Exception), stoppingToken);
            }
    }
}