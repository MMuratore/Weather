namespace Weather.SharedKernel.Domain;

public abstract class Entity<TId> : IHasDomainEvents
    where TId: struct
{
    private readonly List<IDomainEvent> _domainEvents = [];
    
    public TId Id { get; protected set; }
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    public void ClearDomainEvents() => _domainEvents.Clear();
    
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
