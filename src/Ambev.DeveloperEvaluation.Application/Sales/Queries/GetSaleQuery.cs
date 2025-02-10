using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries;

public class GetSaleQuery : IRequest<ApplicationResult<SaleResult>>
{
    public required Guid SaleId { get; init; }
}