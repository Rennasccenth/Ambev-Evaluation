using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUser;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Users.Features;

public sealed class UserTests : BaseTest
{
    public UserTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory) 
        : base(webApplicationFactory) { }

    [Fact(DisplayName = "GET api/users/{userId} when user exists but you are an customer returns 403 Forbidden")]
    public async Task GetUserById_WhenUserExistsButYouAreCustomer_Returns403Forbidden()
    {
        // Arrange
        HttpClient authenticatedClient = await AuthenticateAsAdminAsync();

        User creatingCustomerUser = UserTestData.DumpUser(status: UserStatus.Active, role: UserRole.Customer);
        string userEmail = creatingCustomerUser.Email;
        string password = creatingCustomerUser.Password;
        
        // Creates the Customer User as an ADMIN
        HttpResponseMessage createUserHttpResponse = await authenticatedClient.PostAsJsonAsync(
            requestUri: "api/users",
            value: TestUserBuilder.CreateUserRequest(creatingCustomerUser));
        createUserHttpResponse.EnsureSuccessStatusCode();
        
        HttpResponseMessage getUserHttpResponse = await authenticatedClient.GetAsync(createUserHttpResponse.Headers.Location);
        getUserHttpResponse.EnsureSuccessStatusCode();

        HttpClient authenticatedCustomerClient = await AuthenticateAsCustomerAsync(userEmail, password);
        
        // Act
        HttpResponseMessage getUserResponseWhileCustomerResponse =
            await authenticatedCustomerClient.GetAsync(createUserHttpResponse.Headers.Location);

        // Assert
        using (var _ = new AssertionScope())
        {
            getUserResponseWhileCustomerResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
    
    [Fact(DisplayName = "GET api/users/{userId} when user exists should return 200 Ok containing the user response.")]
    public async Task GetUserById_WhenUserExists_ReturnsOkWithUser()
    {
        // Arrange
        HttpClient authenticatedClient = await AuthenticateAsAdminAsync();

        HttpResponseMessage createUserHttpResponse = await authenticatedClient.PostAsJsonAsync(
            requestUri: "api/users",
            value: TestUserBuilder.CreateUserRequest(UserTestData.DumpUser()));

        // Act
        HttpResponseMessage getUserHttpResponse = await authenticatedClient.GetAsync(createUserHttpResponse.Headers.Location);        

        // Assert
        using (var _ = new AssertionScope())
        {
            getUserHttpResponse.StatusCode.Should()
                .Be(HttpStatusCode.OK, "Once user was created, we should be able to retrieve it by calling this endpoint.");

            var getUserResponse = await getUserHttpResponse.Content.ReadFromJsonAsync<GetUserResponse>();
            getUserResponse.Should()
                .NotBeNull("we should be able to correctly parse the response.");
        }
    }

    [Fact(DisplayName = "GET api/users/{userId} when user doesn't exists should return 404 NotFound.")]
    public async Task GetUserById_WhenUserDoesntExists_ReturnsNotFound()
    {
        // Arrange
        HttpClient authenticatedClient = await AuthenticateAsAdminAsync();
        
        // Act
        HttpResponseMessage response = await authenticatedClient.GetAsync($"api/users/{Guid.NewGuid()}");        

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}