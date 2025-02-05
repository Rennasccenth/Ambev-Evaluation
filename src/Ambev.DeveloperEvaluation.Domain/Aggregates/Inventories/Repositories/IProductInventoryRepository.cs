namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Inventories.Repositories;

public interface IProductInventoryRepository
{
    Task<Dictionary<Guid, int>> GetProductQuantitiesAsync(IEnumerable<Guid> productIds, CancellationToken ct);
    Task<ProductInventory> StockProductQuantityAsync(Guid productId, ulong quantity, CancellationToken ct);
    Task<ProductInventory> DecreaseProductQuantityAsync(Guid productId, ulong quantity, CancellationToken ct);
}