using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Queries.GetCart;

public class GetCartRequest
{
    [FromRoute] public Guid UserId { get; }

    public GetCartRequest(Guid userId)
    {
        UserId = userId;
    }
}