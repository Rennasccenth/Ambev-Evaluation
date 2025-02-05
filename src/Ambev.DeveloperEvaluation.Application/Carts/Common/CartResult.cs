namespace Ambev.DeveloperEvaluation.Application.Carts.Common;

public sealed class CartResult
{
    public required string Id { get; init; }
    public required Guid UserId { get; init; }
    public required DateTime Date { get; init; }
    public required List<ProductSummary> Products { get; init; } = [];
}