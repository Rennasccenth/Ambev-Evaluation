namespace Ambev.DeveloperEvaluation.Domain.Events.Abstractions;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEventsAsync(IEventableEntity eventableEntity);
}