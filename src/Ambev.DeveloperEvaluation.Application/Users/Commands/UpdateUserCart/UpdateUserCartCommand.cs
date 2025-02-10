using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUserCart;

public sealed class UpdateUserCartCommand : IRequest<ApplicationResult<CartResult>>
{
    public required Guid UserId { get; init; }
    public required IEnumerable<ProductSummary> Products { get; init; }
}