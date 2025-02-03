using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;

public sealed class GetUsersQuery : PaginatedQuery, IRequest<ApplicationResult<GetUsersQueryResult>>
{
    public Guid? Id { get; init; }
    public Email? Email { get; init; }
    public string? Username { get; init; }
    public string? Firstname { get; init; }
    public string? Lastname { get; init; }
    public string? Street { get; init; }
    public string? Zipcode { get; init; }
    public int? Number { get; init; }
    public string? City { get; init; }
    public string? Lat { get; init; }
    public string? Long { get; init; }
    public Phone? Phone { get; init; }
    public UserStatus? Status { get; init; }
    public UserRole? Role { get; init; }
}