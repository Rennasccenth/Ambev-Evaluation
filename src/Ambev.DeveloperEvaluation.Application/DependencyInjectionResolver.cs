using System.Reflection;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ambev.DeveloperEvaluation.Application;

public static class DependencyInjectionResolver
{
    public static IServiceCollection InstallApplicationLayer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(config =>
        {
            // Install all mediatR Handlers from Application Assembly
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });
        serviceCollection.TryAddSingleton<IPasswordHasher, BCryptPasswordHasher>();

        // Enable Validation behavior over Commands 
        serviceCollection.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());

        return serviceCollection;
    }
}