using System.Reflection;
using Ambev.DeveloperEvaluation.WebApi.Common.Converters;
using Ambev.DeveloperEvaluation.Common.ExceptionHandlers;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Security;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ambev.DeveloperEvaluation.WebApi;

internal static class DependencyInjectionResolver
{
    internal static IServiceCollection InstallApiDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton(TimeProvider.System);

        // Adds HCs
        serviceCollection.AddBasicHealthChecks();

        // Configure Generation of OpenApiDocumentation
        serviceCollection.AddSwaggerGen();

        // Register Mapping Profiles from API layer
        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Register All AbstractValidators in current Layer
        serviceCollection.AddValidatorsFromAssembly(
            assembly: Assembly.GetExecutingAssembly(),
            includeInternalTypes: true);

        // Configure JWT related Stuff
        serviceCollection.AddJwtAuthentication();
        
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new EmailJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new PhoneJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new PasswordJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new AddressJsonConverter());
        });

        serviceCollection.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                context.ProblemDetails.Extensions.TryAdd("traceId", context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity.TraceId);
            };
        });
        
        // Microsoft recommends this for .NET latest versions like [.NET 8+]
        // See GlobalExceptionHandler docs for more info.
        serviceCollection.AddExceptionHandler<GlobalExceptionHandler>();

        return serviceCollection;
        
    }
}