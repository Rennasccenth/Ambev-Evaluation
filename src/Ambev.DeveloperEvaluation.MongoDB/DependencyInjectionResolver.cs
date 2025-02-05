using Ambev.DeveloperEvaluation.Common.Options;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Inventories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Inventories.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Repositories;
using Ambev.DeveloperEvaluation.MongoDB.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.MongoDB;

public static class DependencyInjectionResolver
{
    public static IServiceCollection InstallMongoDbInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.RegisterOption<MongoDbSettings>(MongoDbSettings.SectionName);
        serviceCollection.AddSingleton<IMongoClient>(serviceProvider => 
        {
            MongoDbSettings settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            
            return new MongoClient(settings.ConnectionString);
        });
        serviceCollection.AddScoped<IMongoDatabase>(serviceProvider => 
        {
            IMongoClient mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
            MongoDbSettings settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;

            return mongoClient.GetDatabase(settings.DatabaseName);
        });

        serviceCollection.AddMongoDbCollections();
        serviceCollection.AddHostedService<MongoIndexInitializer>();
        
        serviceCollection.AddTransient<IProductInventoryRepository, ProductInventoryRepository>();
        serviceCollection.AddTransient<ICartRepository, CartRepository>();
        serviceCollection.AddTransient<ISaleRepository, SaleRepository>();
        serviceCollection.AddTransient<ISaleProductRepository, SaleProductRepository>();

        return serviceCollection;
    }

    private static IServiceCollection AddMongoDbCollections(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IMongoCollection<Cart>>(serviceProvider => 
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<Cart>(GetCollectionName(typeof(Cart)));
        });

        serviceCollection.AddScoped<IMongoCollection<Sale>>(serviceProvider => 
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<Sale>(GetCollectionName(typeof(Sale)));
        });
        
        serviceCollection.AddScoped<IMongoCollection<ProductInventory>>(serviceProvider => 
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<ProductInventory>(GetCollectionName(typeof(ProductInventory)));
        });

        return serviceCollection;
    }

    private static string GetCollectionName(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        
        // probably humanizer can do it better, but that's fine for now.
        return type.Name.ToLowerInvariant().EndsWith('s') 
            ? type.Name.ToLowerInvariant()
            : type.Name.ToLowerInvariant() + 's';
    }
}