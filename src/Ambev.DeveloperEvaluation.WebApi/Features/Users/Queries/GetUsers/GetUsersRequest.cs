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
    [FromQuery] public AddressFilterRequest? Address { get; set; }
    [FromQuery] public string? Phone { get; set; }
    [FromQuery] public string? Status { get; set; }
    [FromQuery] public string? Role { get; set; }
}

public sealed class AddressFilterRequest
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Zipcode { get; set; }
    
    [FromQuery(Name="_minNumber")] public int? MinNumber { get; set; }
    [FromQuery] public int? Number { get; set; }
    [FromQuery(Name="_maxNumber")] public int? MaxNumber { get; set; }

    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
}