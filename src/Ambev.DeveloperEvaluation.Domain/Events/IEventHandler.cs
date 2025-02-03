namespace Ambev.DeveloperEvaluation.Domain.Events;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task Handle(TEvent @event);
}