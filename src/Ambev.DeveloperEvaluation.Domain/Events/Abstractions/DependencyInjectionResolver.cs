using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Domain.Events.Abstractions;

public static class DependencyInjectionResolver
{
    public static IServiceCollection AddEventHandlers(this IServiceCollection services, params Assembly[] assemblies)
    {
        Type handlerType = typeof(IEventHandler<>);

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