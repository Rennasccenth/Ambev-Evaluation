using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC;

public static class DependencyInjectionResolver
{
    /// <summary>
    /// Register dependencies from all related modules.
    /// </summary>
    public static void RegisterDependenciesServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .InstallInfrastructureLayer()
            .InstallApplicationLayer();
    }
}