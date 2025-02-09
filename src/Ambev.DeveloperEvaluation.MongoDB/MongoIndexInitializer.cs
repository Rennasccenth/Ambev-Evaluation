using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Inventories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.MongoDB;

internal sealed class MongoIndexInitializer : IHostedService
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoIndexInitializer> _logger;

    public MongoIndexInitializer(
        IServiceProvider serviceProvider,
        ILogger<MongoIndexInitializer> logger)
    {
        IServiceScope serviceScope = serviceProvider.CreateScope();
        _database = serviceScope.ServiceProvider.GetRequiredService<IMongoDatabase>();
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await CreateCartIndexes(cancellationToken);
            await CreateSaleIndexes(cancellationToken);
            await CreateProductInventoryIndexes(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create MongoDB indexes.");
            throw;
        }
    }

    private async Task CreateCartIndexes(CancellationToken ct)
    {
        var collection = _database.GetCollection<Cart>(GetCollectionName(typeof(Cart)));
        
        var userIdIndex = Builders<Cart>.IndexKeys.Ascending(x => x.UserId);

        CreateIndexOptions indexOptions = new()
        {
            Unique = true,
            Name = "Unique_CustomerId"
        };
        
        var indexModel = new CreateIndexModel<Cart>(userIdIndex, indexOptions);
        await collection.Indexes.CreateOneAsync(indexModel, cancellationToken: ct);
    }

    private async Task CreateSaleIndexes(CancellationToken ct)
    {
        var collection = _database.GetCollection<Sale>(GetCollectionName(typeof(Sale)));

        var compoundIndex = Builders<Sale>.IndexKeys
            .Ascending(x => x.CustomerId)
            .Ascending(x => x.Id);

        var indexOptions = new CreateIndexOptions { Unique = true };

        var indexModel = new CreateIndexModel<Sale>(compoundIndex, indexOptions);

        await collection.Indexes.CreateOneAsync(indexModel, cancellationToken: ct);
    }
    
    private async Task CreateProductInventoryIndexes(CancellationToken ct)
    {
        var collection = _database.GetCollection<ProductInventory>(GetCollectionName(typeof(ProductInventory)));

        var productInventoryIndex = Builders<ProductInventory>.IndexKeys.Ascending(x => x.ProductId);
        CreateIndexOptions indexOptions = new()
        {
            Unique = true,
            Name = "Unique_ProductId"
        };
        var indexModel = new CreateIndexModel<ProductInventory>(productInventoryIndex, indexOptions);

        await collection.Indexes.CreateOneAsync(indexModel, cancellationToken: ct);
    }

    private static string GetCollectionName(Type type) => 
        type.Name.ToLowerInvariant().EndsWith('y') 
            ? type.Name.ToLowerInvariant().TrimEnd('y') + "ies" 
            : type.Name.ToLowerInvariant().EndsWith('s') 
                ? type.Name.ToLowerInvariant() 
                : type.Name.ToLowerInvariant() + "s";

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}