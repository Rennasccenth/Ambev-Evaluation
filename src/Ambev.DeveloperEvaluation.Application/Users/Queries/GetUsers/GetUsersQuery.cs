using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;

public sealed class GetUsersQuery : PaginatedQuery, IRequest<ApplicationResult<GetUsersQueryResult>>;