using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Events;

internal sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IServiceScopeFactory serviceScopeFactory, ILogger<DomainEventDispatcher> logger)
    {
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _logger = logger;
    }

    public async Task DispatchAndClearEventsAsync(IEventableEntity eventableEntity)
    {
        _logger.LogInformation("Dispatching {DomainEventsCount} events.", eventableEntity.DomainEvents.Count);

        ArgumentNullException.ThrowIfNull(eventableEntity);

        using var scope = _serviceScopeFactory.CreateScope();

        List<Task> handlingTasks = [];
        foreach (IEvent domainEvent in eventableEntity.DomainEvents)
        {
            Type handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());

            IEventHandler<IEvent>[] registeredHandlers = scope
                .ServiceProvider
                .GetServices(handlerType)
                .OfType<IEventHandler<IEvent>>()
                .ToArray();

            if (registeredHandlers.Length is 0)
            {
                _logger.LogWarning("No event handlers registered for event {EventName}.", domainEvent.GetType().Name);
                continue;
            }

            handlingTasks.AddRange(registeredHandlers
                .Select(eventHandler => eventHandler.Handle(domainEvent)));

            await Task.WhenAll(handlingTasks);
        }

        eventableEntity.ClearDomainEvents();
        _logger.LogInformation("Dispatched {DomainEventsCount} events.", handlingTasks.Count);
    }
}