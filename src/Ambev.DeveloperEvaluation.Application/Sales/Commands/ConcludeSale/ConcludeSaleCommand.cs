using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.ConcludeSale;

public sealed class ConcludeSaleCommand : IRequest<ApplicationResult<SaleResult>>
{
    public Guid SaleId { get; init; }
}