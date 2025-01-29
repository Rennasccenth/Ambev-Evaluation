using System.Reflection;
using Ambev.DeveloperEvaluation.Common.ExceptionHandlers;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.WebApi;

internal static class DependencyInjectionResolver
{
    internal static IServiceCollection InstallApiDependencies(this IServiceCollection serviceCollection)
    {
        // Adds HCs
        serviceCollection.AddBasicHealthChecks();

        // Configure Generation of OpenApiDocumentation
        serviceCollection.AddSwaggerGen();

        // Register Mapping Profiles from API layer
        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Configure JWT related Stuff
        serviceCollection.AddJwtAuthentication();
        
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddControllers();

        serviceCollection.AddProblemDetails();
        
        // Microsoft recommends this for .NET latest versions like [.NET 8+]
        // See GlobalExceptionHandler docs for more info.
        serviceCollection // Be aware, order matters
            .AddExceptionHandler<ValidationExceptionHandler>()
            .AddExceptionHandler<GlobalExceptionHandler>();

        return serviceCollection;
        
    }
}