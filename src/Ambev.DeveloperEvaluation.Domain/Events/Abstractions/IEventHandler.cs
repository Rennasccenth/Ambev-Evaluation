namespace Ambev.DeveloperEvaluation.Domain.Events.Abstractions;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent @event);
}
