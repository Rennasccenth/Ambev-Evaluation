namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

public sealed class ProductQuantifier
{
    public required Guid ProductId { get; set; }
    public required int Quantity { get; set; }
}