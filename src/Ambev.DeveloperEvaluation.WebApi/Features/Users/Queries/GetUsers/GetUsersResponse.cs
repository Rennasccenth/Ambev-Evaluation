using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUsers;

public sealed class GetUsersResponse : PaginatedResponse<GetUsersRequestItem>;

public sealed class GetUsersRequestItem
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
    public required Address Address { get; init; }
    public required string Phone { get; init; }
    public required string Status { get; init; }
    public required string Role { get; init; }
}