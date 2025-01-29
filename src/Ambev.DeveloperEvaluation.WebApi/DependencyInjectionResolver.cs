using System.Reflection;
using Ambev.DeveloperEvaluation.Common.ExceptionHandlers;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Security;
using Microsoft.AspNetCore.Http.Features;

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

        serviceCollection.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
                context.ProblemDetails.Extensions.Add("traceId", context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity.Id);
            };
        });
        
        // Microsoft recommends this for .NET latest versions like [.NET 8+]
        // See GlobalExceptionHandler docs for more info.
        serviceCollection.AddExceptionHandler<GlobalExceptionHandler>();

        return serviceCollection;
        
    }
}