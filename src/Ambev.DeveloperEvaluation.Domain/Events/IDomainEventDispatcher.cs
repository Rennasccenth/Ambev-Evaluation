namespace Ambev.DeveloperEvaluation.Domain.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEventsAsync(IEventableEntity eventableEntity);
}