using System.Text.Json;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

namespace Ambev.DeveloperEvaluation.Functional.Users;

public static class TestUserBuilder
{
    
    public static CreateUserRequest GetInvalidUserRequest()
    {
        var invalidRequest = """
                           {
                             "Username": "XXXXXXXX",
                             "Password": "XXXXXXXXXXXXX!",
                             "Phone": "00000000000000",
                             "Email": "john.doe@example.com",
                             "Name": {
                               "Firstname": "John",
                               "Lastname": "Doe"
                             },
                             "Address": {
                               "City": "São Paulo",
                               "Street": "Av. Paulista",
                               "Number": 12,
                               "Zipcode": "01311-200",
                               "Geolocation": {
                                 "Lat": "-23.5631",
                                 "Long": "-46.6567"
                               }
                             },
                             "Status": "Active",
                             "Role": "Admin"
                           }
                           """;

        return JsonSerializer.Deserialize<CreateUserRequest>(invalidRequest) ?? throw new InvalidOperationException();
    }

     public static CreateUserRequest GetInvalidUserRequestWithEmptyUsername()
    {
        var invalidRequest = """
                           {
                             "Username": "",
                             "Password": "XXXXXXXXXXXXX!",
                             "Phone": "00000000000000",
                             "Email": "john.doe@example.com",
                             "Name": {
                               "Firstname": "John",
                               "Lastname": "Doe"
                             },
                             "Address": {
                               "City": "São Paulo",
                               "Street": "Av. Paulista",
                               "Number": 12,
                               "Zipcode": "01311-200",
                               "Geolocation": {
                                 "Lat": "-23.5631",
                                 "Long": "-46.6567"
                               }
                             },
                             "Status": "Active",
                             "Role": "Admin"
                           }
                           """;

        return JsonSerializer.Deserialize<CreateUserRequest>(invalidRequest) ?? throw new InvalidOperationException();
    }

    public static CreateUserRequest GetValidUserRequest()
    {
        // var validFaker = new Faker<CreateUserRequest>("pt_BR")
        //     .RuleFor(u => u.Email, f => f.Internet.Email())
        //     .RuleFor(u => u.Username, f => f.Internet.UserName())
        //     .RuleFor(u => u.Password, f => f.Internet.Password(8) + "!192")
        //     .RuleFor(u => u.Name, f => new NameDto(
        //         f.Name.FirstName(),
        //         f.Name.LastName())
        //     )
        //     .RuleFor(u => u.Address, f => new AddressDto
        //     {
        //         City = f.Address.City(),
        //         Street = f.Address.StreetAddress(),
        //         Number = int.Parse(f.Address.BuildingNumber()),
        //         Zipcode = f.Address.City(),
        //         Geolocation = new GeolocationDto
        //         {
        //             Lat = f.Address.Latitude().ToString(CultureInfo.InvariantCulture),
        //             Long = f.Address.Longitude().ToString(CultureInfo.InvariantCulture),
        //         }
        //     })
        //     .RuleFor(u => u.Phone, "65981828384")
        //     .RuleFor(u => u.Status, f => f.PickRandom("Active", "Inactive", "Suspended"))
        //     .RuleFor(u => u.Role, f => f.PickRandom("Customer", "Manager", "Admin"));
        //
        // return validFaker.Generate();

        var validRequest = """
                           {
                             "Username": "john_doe",
                             "Password": "SecurePass123!",
                             "Phone": "+5511999999999",
                             "Email": "john.doe@example.com",
                             "Name": {
                               "Firstname": "John",
                               "Lastname": "Doe"
                             },
                             "Address": {
                               "City": "São Paulo",
                               "Street": "Av. Paulista",
                               "Number": 12,
                               "Zipcode": "01311-200",
                               "Geolocation": {
                                 "Lat": "-23.5631",
                                 "Long": "-46.6567"
                               }
                             },
                             "Status": "Active",
                             "Role": "Admin"
                           }
                           """;

        return JsonSerializer.Deserialize<CreateUserRequest>(validRequest) ?? throw new InvalidOperationException();
    }
}

