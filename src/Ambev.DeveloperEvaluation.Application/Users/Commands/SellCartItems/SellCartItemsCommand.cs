using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.SellCartItems;


public sealed class SellCartItemsCommand : IRequest<ApplicationResult<SaleResult>>
{
    public required Guid UserId { get; init; }
    public required string BranchName { get; init; }
}