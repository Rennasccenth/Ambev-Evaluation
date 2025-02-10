namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

public sealed class UpdateCartProductsRequest
{
    public required IEnumerable<ProductQuantifier> Products { get; init; }
}