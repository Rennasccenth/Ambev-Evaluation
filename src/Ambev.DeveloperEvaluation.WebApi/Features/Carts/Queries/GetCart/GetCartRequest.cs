namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Queries.GetCart;

public class GetCartRequest
{
    public Guid? CartId { get; }
    
    public GetCartRequest(Guid cartId)
    {
        CartId = cartId;
    }
}