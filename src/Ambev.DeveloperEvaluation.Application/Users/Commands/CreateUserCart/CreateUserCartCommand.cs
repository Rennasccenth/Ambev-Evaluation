using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUserCart;

public sealed class CreateUserCartCommand : IRequest<ApplicationResult<CartResult>>
{
    public required Guid UserId { get; init; }
    public required DateTime Date { get; init; }
    public List<ProductSummary> Products { get; init; } = [];
}