using Ambev.DeveloperEvaluation.Common.Options;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Products.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.ORM;

public static class DependencyInjectionResolver
{
    public static IServiceCollection InstallPostgreSqlInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.RegisterOption<PostgreSqlSettings>(PostgreSqlSettings.SectionName);
        // Use this instead DI default methods for safer DbContext injection.
        serviceCollection.AddDbContext<DefaultContext>((provider, options) =>
        {
            PostgreSqlSettings postgreSqlSettings = provider.GetRequiredService<IOptions<PostgreSqlSettings>>().Value;
            
            options.UseNpgsql(postgreSqlSettings.ConnectionString, builder =>
            {
                builder.EnableRetryOnFailure(
                    maxRetryCount: (int)postgreSqlSettings.MaxRetryCount,
                    maxRetryDelay: TimeSpan.FromSeconds((int)postgreSqlSettings.RetryDelayInSeconds),
                    errorCodesToAdd: null);
            });

            IWebHostEnvironment webHostEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
            if (webHostEnvironment.IsProduction()) return;

            if (postgreSqlSettings.EnableDetailedErrors)
            {
                options.EnableDetailedErrors();
            }
            if (postgreSqlSettings.EnableSensitiveDataLogging)
            {
                options.EnableSensitiveDataLogging();
            }
        }, ServiceLifetime.Transient);

        serviceCollection.AddTransient<IUserRepository, UserRepository>();
        serviceCollection.AddTransient<IProductRegistryRepository, ProductRegistryRepository>();

        serviceCollection.AddHostedService<EnsureDatabaseCreatedHostService>();        
        return serviceCollection;
    }
}