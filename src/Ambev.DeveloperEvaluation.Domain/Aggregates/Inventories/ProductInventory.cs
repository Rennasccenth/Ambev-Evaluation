using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Inventories;

public sealed class ProductInventory : BaseEntity
{
    public int Quantity { get; private set; }

    public ProductInventory(Guid productId, int initialQuantity)
    {
        Id = productId;
        Quantity = initialQuantity;
    }
}