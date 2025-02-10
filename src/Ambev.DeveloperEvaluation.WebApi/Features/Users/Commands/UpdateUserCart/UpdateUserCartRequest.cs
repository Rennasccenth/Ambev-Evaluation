using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUserCart;

public sealed class UpdateUserCartRequest
{
    public Guid UserId { get; init; }
    public IEnumerable<ProductQuantifier> Products { get; init; }

    public UpdateUserCartRequest(Guid userId, IEnumerable<ProductQuantifier> products)
    {
        UserId = userId;
        Products = products;
    }
}