using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUser;


public sealed class UpdateUserRequestBody
{
    [FromBody] public required string Username { get; init; }
    [FromBody] public required string Password { get; init; }
    [FromBody] public required string Phone { get; init; }
    [FromBody] public required string Email { get; init; }
    [FromBody] public required UpdatingAddress Address { get; init; }
    [FromBody] public required string Status { get; init; }
    [FromBody] public required string Role { get; init; }
}

public sealed class UpdateUserRequest
{
    public UpdateUserRequest(Guid id, UpdateUserRequestBody requestBody)
    {
        Id = id;
        Username = requestBody.Username;
        Password = requestBody.Password;
        Phone = requestBody.Phone;
        Email = requestBody.Email;
        Address = requestBody.Address;
        Status = requestBody.Status;
        Role = requestBody.Role;
    }
    public Guid Id { get; }
    public string Username { get; }
    public string Password { get; }
    public string Phone { get; }
    public string Email { get; }
    public UpdatingAddress Address { get; }
    public string Status { get; }
    public string Role { get; }
}

public sealed record UpdatingAddress
{
    public required string City { get; init; }
    public required string Street { get; init; }
    public required int Number { get; init; }
    public required string Zipcode { get; init; }
    public required NewGeolocation Geolocation { get; init; }
}

public sealed record NewGeolocation
{
    public required string Lat { get; init; }
    public required string Long { get; init; }
}

public sealed record NameDto
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
}