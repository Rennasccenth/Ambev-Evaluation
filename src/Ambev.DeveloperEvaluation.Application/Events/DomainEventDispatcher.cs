using System.Reflection;
using Ambev.DeveloperEvaluation.Domain.Events.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Events;

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
        if (eventableEntity.DomainEvents.Count == 0) return;

        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        List<Task> handlingTasks = [];

        foreach (IEvent @event in eventableEntity.DomainEvents)
        {
            Type handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = scope.ServiceProvider.GetServices(handlerType)
                .ToList();

            if (handlers.Count == 0)
            {
                _logger.LogWarning("No handlers registered for event {EventType}", @event.GetType().Name);
                continue;
            }

            foreach (var handler in handlers)
            {
                MethodInfo? handleMethod = handlerType.GetMethod("HandleAsync");
                if (handleMethod == null)
                {
                    _logger.LogError("Handler {HandlerType} does not implement HandleAsync method", handler?.GetType().Name);
                    continue;
                }

                // TODO: I guess I can add the CancellationToken in params list, but i need to check it later. 
                handlingTasks.Add((Task)handleMethod.Invoke(handler, [@event])!);
            }
        }

        await Task.WhenAll(handlingTasks);
        eventableEntity.ClearDomainEvents();
    }
}