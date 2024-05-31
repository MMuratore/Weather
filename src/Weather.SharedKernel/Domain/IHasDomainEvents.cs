namespace Weather.SharedKernel.Domain;

public interface IHasDomainEvents
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    public void ClearDomainEvents();
}