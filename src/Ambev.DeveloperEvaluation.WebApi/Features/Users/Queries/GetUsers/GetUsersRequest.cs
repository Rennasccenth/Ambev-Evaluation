using Ambev.DeveloperEvaluation.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUsers;

public sealed class GetUsersRequest : PaginatedRequest
{
    [FromQuery] public Guid? Id { get; set; }
    [FromQuery] public string? Email { get; set; }
    [FromQuery] public string? Username { get; set; }
    [FromQuery] public string? Firstname { get; set; }
    [FromQuery] public string? Lastname { get; set; }
    [FromQuery] public string? Street { get; set; }
    [FromQuery] public int? Number { get; set; }
    [FromQuery] public string? City { get; set; }
    [FromQuery] public string? Lat { get; set; }
    [FromQuery] public string? Long { get; set; }
    [FromQuery] public string? Phone { get; set; }
    [FromQuery] public string? Status { get; set; }
    [FromQuery] public string? Role { get; set; }
}