using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        // Use this instead DI default methods for safer DbContext injection.
        builder.Services.AddDbContext<DefaultContext>((provider, options) =>
        {
            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
            string? connString = configuration.GetConnectionString(name: "DefaultConnection");
            ArgumentException.ThrowIfNullOrEmpty(connString);

            options.UseNpgsql(connString);
        });
        builder.Services.AddScoped<IUserRepository, UserRepository>();
    }
}