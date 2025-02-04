using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using Ambev.DeveloperEvaluation.MongoDB.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.MongoDB;

public static class DependencyInjectionResolver
{
    public static IServiceCollection InstallMongoDbInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IProductInventoryRepository, ProductInventoryRepository>();

        return serviceCollection;
    }
}