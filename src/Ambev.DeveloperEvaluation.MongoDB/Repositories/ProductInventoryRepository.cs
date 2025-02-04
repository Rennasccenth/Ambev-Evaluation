using Ambev.DeveloperEvaluation.Domain.Aggregates.Inventories;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.MongoDB.Repositories;

internal sealed class ProductInventoryRepository : IProductInventoryRepository
{
    private readonly ILogger<ProductInventoryRepository> _logger;
    private readonly IMongoCollection<ProductInventory> _inventoryCollection;

    public ProductInventoryRepository(IMongoCollection<ProductInventory> inventoryCollection,
        ILogger<ProductInventoryRepository> logger)
    {
        _logger = logger;
        _inventoryCollection = inventoryCollection;
    }

    public async Task<Dictionary<Guid, int>> GetProductQuantitiesAsync(IEnumerable<Guid> productIds, CancellationToken ct)
    {
        var filter = Builders<ProductInventory>.Filter.In(pi => pi.Id, productIds);
        
        var productInventories = await _inventoryCollection.Find(filter).ToListAsync(ct);
        
        return productInventories.ToDictionary(pi => pi.Id, pi => pi.Quantity);
    }

    public async Task<ProductInventory> StockProductQuantityAsync(Guid productId, ulong quantity, CancellationToken ct)
    {
        _logger.LogInformation("ReStocking product {ProductId} with quantity {Quantity}", productId, quantity);

        var filter = Builders<ProductInventory>.Filter.Eq(pi => pi.Id, productId);
        ProductInventory reStockedProduct = await _inventoryCollection.FindOneAndUpdateAsync(
            filter, 
            Builders<ProductInventory>.Update.Inc(pi => pi.Quantity, (long)quantity),
            new FindOneAndUpdateOptions<ProductInventory>
            {
                ReturnDocument = ReturnDocument.After
            }, 
            ct);

        _logger.LogInformation("Product {ProductId} quantity is now {Quantity}", productId, reStockedProduct.Quantity);
        return reStockedProduct;
    }

    public async Task<ProductInventory> DecreaseProductQuantityAsync(Guid productId, ulong quantity, CancellationToken ct)
    {
        _logger.LogInformation("Emptying product {ProductId} with quantity {Quantity}", productId, quantity);

        var filter = Builders<ProductInventory>.Filter.Eq(pi => pi.Id, productId);
        ProductInventory reStockedProduct = await _inventoryCollection.FindOneAndUpdateAsync(
            filter, 
            Builders<ProductInventory>.Update.Inc(pi => pi.Quantity, (long)quantity*-1),
            new FindOneAndUpdateOptions<ProductInventory>
            {
                ReturnDocument = ReturnDocument.After
            }, 
            ct);

        _logger.LogInformation("Emptied Product {ProductId} quantity is now {Quantity}", productId, reStockedProduct.Quantity);
        return reStockedProduct;
    }
}