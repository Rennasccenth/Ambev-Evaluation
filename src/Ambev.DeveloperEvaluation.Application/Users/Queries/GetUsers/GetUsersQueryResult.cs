using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;

public sealed class GetUsersQueryResult
{
    public required List<GetUsersQuerySummary> Users { get; init; }
    public required int TotalCount { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalPages { get; init; }
}

public sealed class GetUsersQuerySummary
{
    public required Guid Id { get; init; }
    public required Email Email { get; init; }
    public required string Username { get; init; }
    public required Password Password { get; init; }
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
    public required Address Address { get; init; }
    public required Phone Phone { get; init; }
    public required UserStatus Status { get; init; }
    public required UserRole Role { get; init; }
}