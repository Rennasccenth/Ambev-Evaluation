using System.Reflection;
using Ambev.DeveloperEvaluation.Domain.Events;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ambev.DeveloperEvaluation.Domain;

public static class DependencyInjectionResolver
{
    public static IServiceCollection RegisterDomainServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddTransient<IDomainEventDispatcher, DomainEventDispatcher>();

        serviceCollection.AddValidatorsFromAssembly(
            assembly: Assembly.GetExecutingAssembly(),
            includeInternalTypes: true);

        return serviceCollection;
    }
}