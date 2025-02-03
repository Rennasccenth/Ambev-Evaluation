using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using Ambev.DeveloperEvaluation.Domain.Repositories.User;
using Ambev.DeveloperEvaluation.ORM.Events;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.ORM;

public static class DependencyInjectionResolver
{
    public static IServiceCollection InstallInfrastructureLayer(this IServiceCollection serviceCollection)
    {
        // Use this instead DI default methods for safer DbContext injection.
        serviceCollection.AddDbContext<DefaultContext>((provider, options) =>
        {
            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
            string? connString = configuration.GetConnectionString(name: "DefaultConnection");
            ArgumentException.ThrowIfNullOrEmpty(connString);

            options.UseNpgsql(connString);

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development") return;

            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        }, ServiceLifetime.Transient);

        serviceCollection.AddTransient<IUserRepository, UserRepository>();
        serviceCollection.AddTransient<IProductRepository, ProductRepository>();
        serviceCollection.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return serviceCollection;
    }
}