using System.ComponentModel;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUser;

namespace Ambev.DeveloperEvaluation.Functional.Features.Users;

public static class TestUserBuilder
{
    public static Task CreateUserAsync(User userEntity, HttpClient httpClient)
    {
        var createUserRequest = new CreateUserRequest
        {
            Username = userEntity.Username,
            Password = userEntity.Password,
            Name = new NameDto
            {
                Firstname = userEntity.Firstname,
                Lastname = userEntity.Lastname
            },
            Email = userEntity.Email,
            Phone = userEntity.Phone,
            Address = new AddressDto
            {
                City = userEntity.Address.City,
                Street = userEntity.Address.Street,
                Number = userEntity.Address.Number,
                Zipcode = userEntity.Address.ZipCode,
                Geolocation = new GeolocationDto
                {
                    Lat = userEntity.Address.Latitude,
                    Long = userEntity.Address.Longitude
                }
            },
            Status = new EnumConverter(typeof(UserStatus)).ConvertToString(userEntity.Status)!,
            Role = new EnumConverter(typeof(UserRole)).ConvertToString(userEntity.Role)!,
        };

        return httpClient.PostAsJsonAsync("/api/users", createUserRequest)
            .ContinueWith(task => task.Result.EnsureSuccessStatusCode());
    }
    
    public static CreateUserRequest CreateUserRequest(User userEntity)
    {
        return new CreateUserRequest
        {
            Username = userEntity.Username,
            Password = userEntity.Password,
            Name = new NameDto
            {
                Firstname = userEntity.Firstname,
                Lastname = userEntity.Lastname
            },
            Email = userEntity.Email,
            Phone = userEntity.Phone,
            Address = new AddressDto
            {
                City = userEntity.Address.City,
                Street = userEntity.Address.Street,
                Number = userEntity.Address.Number,
                Zipcode = userEntity.Address.ZipCode,
                Geolocation = new GeolocationDto
                {
                    Lat = userEntity.Address.Latitude,
                    Long = userEntity.Address.Longitude
                }
            },
            Status = new EnumConverter(typeof(UserStatus)).ConvertToString(userEntity.Status)!,
            Role = new EnumConverter(typeof(UserRole)).ConvertToString(userEntity.Role)!,
        };
    }
}

