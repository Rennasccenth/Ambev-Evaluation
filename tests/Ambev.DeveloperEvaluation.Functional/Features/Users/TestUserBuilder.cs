using System.Text.Json;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

namespace Ambev.DeveloperEvaluation.Functional.Features.Users;

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
        const string validRequest = """
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
    
    public static CreateUserRequest GetInactiveUserRequest()
    {
        const string validRequest = """
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
                                      "Status": "Inactive",
                                      "Role": "Admin"
                                    }
                                    """;

        return JsonSerializer.Deserialize<CreateUserRequest>(validRequest) ?? throw new InvalidOperationException();
    }
}

