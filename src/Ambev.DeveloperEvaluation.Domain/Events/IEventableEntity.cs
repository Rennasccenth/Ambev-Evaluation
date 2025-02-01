namespace Ambev.DeveloperEvaluation.Domain.Events;

public interface IEventableEntity
{
    List<IEvent> DomainEvents { get; }
}