using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCart;

public sealed class GetCartQuery : IRequest<ApplicationResult<CartResult>>
{
    public Guid CartId { get; init; }

    public GetCartQuery(Guid cartId)
    {
        CartId = cartId;
    }
}