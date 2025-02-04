using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Inventories;

public sealed class ProductInventory : BaseEntity
{
    public Guid ProductId { get; set; }
    public int Quantity { get; private set; }
}