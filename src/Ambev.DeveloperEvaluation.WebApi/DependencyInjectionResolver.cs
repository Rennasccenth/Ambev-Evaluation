using System.Reflection;
using Ambev.DeveloperEvaluation.Common.ExceptionHandlers;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Options;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Abstractions;
using Ambev.DeveloperEvaluation.Domain.ValueObjects.JsonConverters;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace Ambev.DeveloperEvaluation.WebApi;

internal static class DependencyInjectionResolver
{
    internal static IServiceCollection InstallApiDependencies(this IServiceCollection serviceCollection,
        IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        serviceCollection.TryAddSingleton(TimeProvider.System);

        // Adds HCs
        serviceCollection.AddBasicHealthChecks();

        // Configure Generation of OpenApiDocumentation
        serviceCollection.AddSwaggerGen(options =>
        {
            // Gets the last 2 words of type namespace, avoiding type name collision.
            options.CustomSchemaIds(type => string.Join(": ", type.ToString().Split('.').TakeLast(2)));
        
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT Bearer token in the format: Bearer {your_token}"
            });
        
            // Add security requirement to enforce authentication in Swagger UI
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            OpenApiDocumentation openApiDocumentation = new() { Title = "", Description = "" };
            configuration.GetSection(OpenApiDocumentation.SectionName).Bind(openApiDocumentation);
            var descriptionPath = Path.Combine(webHostEnvironment.ContentRootPath, "Docs/OpenApiDescription.md");
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = openApiDocumentation.Title,
                Description = File.ReadAllText(descriptionPath)
            });
        });

        serviceCollection.AddOpenApiDocumentation();
        
        // Register Mapping Profiles from API layer
        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Register All AbstractValidators in current Layer
        serviceCollection.AddValidatorsFromAssembly(
            assembly: Assembly.GetExecutingAssembly(),
            includeInternalTypes: true);

        // Configure JWT related Stuff
        serviceCollection.AddJwtAuthentication(configuration);
        
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new EmailJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new PhoneJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new PasswordJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new AddressJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new RatingJsonConverter());
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

        // Allow us to track the owner of the current request for every layer down bellow
        // (bcs we rely in IUserContext interface)
        serviceCollection
            .AddHttpContextAccessor()
            .AddScoped<IUserContext, UserContext>();
        return serviceCollection;
    }

    public static WebApplication UseOpenApiDocumentation(this WebApplication webApplication,
        ScalarTheme scalarTheme = ScalarTheme.Kepler,
        bool enableDarkMode = true)
    {
        const string openApiDocumentPath = "/openapi/{documentName}.json";

        webApplication.UseOpenApi(settings =>
        {
            settings.Path = openApiDocumentPath;
        });
        
        webApplication.MapScalarApiReference("/docs/", (options, context) =>
        {
            options
                .WithPreferredScheme("Bearer")
                .WithHttpBearerAuthentication(x =>
                {
                    x.Token = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                });

            options
                .WithOpenApiRoutePattern(openApiDocumentPath)
                .WithTheme(scalarTheme)
                .WithDarkMode(enableDarkMode);
        });

        return webApplication;
    }

    private static IServiceCollection AddOpenApiDocumentation(this IServiceCollection serviceCollection)
    {
        serviceCollection.RegisterOption<OpenApiDocumentation>(OpenApiDocumentation.SectionName);

        serviceCollection.AddOpenApiDocument((generatorSettings, provider) =>
        {
            OpenApiDocumentation documentationOptions = provider
                .GetRequiredService<IOptions<OpenApiDocumentation>>().Value;

            generatorSettings.Title = documentationOptions.Title;
            generatorSettings.Description = documentationOptions.Description;
        });

        return serviceCollection;
    }
}