using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands.CreateCart;

public sealed class CreateCartCommand : IRequest<ApplicationResult<CartResult>>
{
    public required DateTime Date { get; init; }
    public List<ProductSummary> Products { get; init; } = [];
}