namespace Ambev.DeveloperEvaluation.Application.Carts.Common;

public sealed class ProductSummary
{
    public required Guid ProductId { get; set; }
    public required int Quantity { get; set; }
}