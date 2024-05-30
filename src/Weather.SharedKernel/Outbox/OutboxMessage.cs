namespace Weather.SharedKernel.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }
    public required string Type { get; init; }
    public required string Content { get; init; }
    public DateTimeOffset CreationTime { get; init; }
    public DateTimeOffset? CompleteTime { get; init; }
    public string? Exception { get; init; }
}
