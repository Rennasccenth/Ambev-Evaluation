using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.UpdateCart;

public sealed class UpdateCartRequest
{
    public Guid CartId { get; init; }
    public IEnumerable<ProductQuantifier> Products { get; init; }

    public UpdateCartRequest(Guid cartId, IEnumerable<ProductQuantifier> products)
    {
        CartId = cartId;
        Products = products;
    }
}