using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProducts;

public class GetProductsQuery : PaginatedQuery, IRequest<ApplicationResult<GetProductsQueryResult>>;