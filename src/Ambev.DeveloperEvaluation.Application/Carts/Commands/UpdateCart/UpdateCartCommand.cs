using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands.UpdateCart;

public sealed class UpdateCartCommand : IRequest<ApplicationResult<CartResult>>
{
    public required Guid CartId { get; init; }
    public IEnumerable<ProductSummary> Products { get; init; } = [];
}