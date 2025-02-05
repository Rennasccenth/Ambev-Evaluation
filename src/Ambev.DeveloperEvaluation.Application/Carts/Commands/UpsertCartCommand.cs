using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands;

public sealed class UpsertCartCommand : IRequest<ApplicationResult<CartResult>>
{
    public required Guid UserId { get; init; }
    public required string Date { get; init; }
    public required List<ProductSummary> Products { get; init; }
}