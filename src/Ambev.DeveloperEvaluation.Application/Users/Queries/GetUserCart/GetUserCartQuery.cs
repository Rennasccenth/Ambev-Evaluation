using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUserCart;

public sealed class GetUserCartQuery : IRequest<ApplicationResult<CartResult>>
{
    public Guid UserId { get; init; }
}