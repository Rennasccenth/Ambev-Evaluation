namespace Ambev.DeveloperEvaluation.Domain.Events.Abstractions;

public interface IEventableEntity
{
    IReadOnlyList<IEvent> DomainEvents { get; }
    void AddDomainEvent(IEvent domainEvent);
    void RemoveDomainEvent(Guid eventId);
    void ClearDomainEvents();
}