using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale;

public sealed class CancelSaleCommand : IRequest<ApplicationResult<SaleResult>>
{
    public Guid SaleId { get; init; }
}