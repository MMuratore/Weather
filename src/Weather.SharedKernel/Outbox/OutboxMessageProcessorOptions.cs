namespace Weather.SharedKernel.Outbox;

public class OutboxMessageProcessorOptions
{
    public const string Section = "OutboxMessageProcessor";
    public int? MaximumConcurrentMessage { get; set; }
    public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(15);
}