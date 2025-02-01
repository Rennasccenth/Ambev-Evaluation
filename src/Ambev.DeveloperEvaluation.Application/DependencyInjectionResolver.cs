using System.Reflection;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ambev.DeveloperEvaluation.Application;

public static class DependencyInjectionResolver
{
    public static IServiceCollection InstallApplicationLayer(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<IPasswordHasher, BCryptPasswordHasher>();

        serviceCollection.AddValidatorsFromAssembly(
            assembly: Assembly.GetExecutingAssembly(),
            includeInternalTypes: true);
        
        serviceCollection.AddMediatR(config =>
        {
            // Install all mediatR Handlers from Application Assembly
            config.RegisterGenericHandlers = true;
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(CommandValidationBehavior<,>));
        });
        
        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());

        return serviceCollection;
    }
}