using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Domain;

public static class DependencyInjectionResolver
{
    public static IServiceCollection RegisterDomainValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidatorsFromAssembly(
            assembly: Assembly.GetExecutingAssembly(),
            includeInternalTypes: true);

        return serviceCollection;
    }
}