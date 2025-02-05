using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public static class EventHandlerIoCResolver
{
    public static IServiceCollection AddEventHandlers(this IServiceCollection services, params Assembly[] assemblies)
    {
        Type handlerType = typeof(IEventHandler<>);

        // We could add Scrutor to handle this gracefully 
        // But basically, get all implementations of IEventHandler with their respective concretes types.
        // E.g: IEventHandler<IEvent> is not valid, since IEvent is not a concrete type.
        // IEventHandler<SomeEventThatImplementsIEvent> this one works!
        var handlers = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type is { IsAbstract: false, IsInterface: false })
            .SelectMany(type => type.GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == handlerType)
                .Select(i => new { ConcreteType = type, Interface = i }))
            .ToList();
        
        foreach (var handler in handlers)
        {
            services.AddTransient(handler.Interface, handler.ConcreteType);
        }

        return services;
    }
}