namespace Ambev.DeveloperEvaluation.Domain.Events;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent @event);
}