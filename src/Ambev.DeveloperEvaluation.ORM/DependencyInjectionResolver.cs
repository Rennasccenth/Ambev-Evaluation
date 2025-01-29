using Ambev.DeveloperEvaluation.Domain.Repositories;
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
        });
        serviceCollection.AddScoped<IUserRepository, UserRepository>();

        return serviceCollection;
    }
}