namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.DeleteCart;

public sealed class DeleteCartRequest
{
    public Guid CartId { get; }

    public DeleteCartRequest(Guid cartId)
    {
        CartId = cartId;
    }
}