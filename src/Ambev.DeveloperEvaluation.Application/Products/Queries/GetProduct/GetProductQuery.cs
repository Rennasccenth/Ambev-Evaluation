using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProduct;

public sealed class GetProductQuery : IRequest<ApplicationResult<GetProductQueryResult>>
{
    public required Guid Id { get; init; }
}
