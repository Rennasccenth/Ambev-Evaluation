using System.Reflection;
using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Events.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ambev.DeveloperEvaluation.Application;

public static class DependencyInjectionResolver
{
    public static IServiceCollection InstallApplicationLayer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        serviceCollection.TryAddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        serviceCollection.AddEventHandlers(Assembly.GetExecutingAssembly());
        
        serviceCollection.AddValidatorsFromAssembly(
            assembly: Assembly.GetExecutingAssembly(),
            includeInternalTypes: true);
        
        serviceCollection.AddMediatR(config =>
        {
            // Install all mediatR Handlers from Application Assembly
            config.RegisterGenericHandlers = true;
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ApplicationValidationBehavior<,>));
        });
        
        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());

        return serviceCollection;
    }
}