using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.User;

public sealed class GetUsersQueryFilter
{
    public int CurrentPage { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? OrderBy { get; init; }
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