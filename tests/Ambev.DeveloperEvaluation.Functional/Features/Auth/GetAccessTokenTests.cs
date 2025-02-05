using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Functional.Features.Users;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUser;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Auth;

public sealed class GetAccessTokenTests : BaseTest
{
    public GetAccessTokenTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory) 
        : base(webApplicationFactory) { }
    
    [Fact(DisplayName = "Get Access Token when user doesn't exists in system, returns 401 Unauthorized")]
    public async Task GetAccessToken_WhenUserDoesntExists_ReturnsUnauthorized()
    {
        // Arrange
        CancellationTokenSource cancellationTokenSource = new();

        // Act
        HttpResponseMessage authenticateUserResponseMessage = await TestServerHttpClient.PostAsJsonAsync(
            requestUri: "api/auth",
            value: new AuthenticateUserRequest
            {
                Email = "asdasd@email.com",
                Password = "Passwor123!"
            },
            cancellationToken: cancellationTokenSource.Token);

        // Assert
        using (var _ = new AssertionScope())
        {
            authenticateUserResponseMessage.StatusCode.Should()
                .Be(HttpStatusCode.Unauthorized);
        }
    }

    [Fact(DisplayName = "Get Access Token when user is active, returns 200 Ok and the generated token.")]
    public async Task GetAccessToken_WhenUserExistsAndIsActive_ReturnsOk()
    {
        // Arrange
        CancellationTokenSource cancellationTokenSource = new();

        var createUserRequest = TestUserBuilder.CreateUserRequest(UserTestData.DumpUser(status: UserStatus.Active));
        string preHashPassword = createUserRequest.Password;

        // Create the user
        HttpResponseMessage createUserHttpResponse = await TestServerHttpClient.PostAsJsonAsync(
            requestUri: "api/users",
            value: createUserRequest,
            cancellationToken: cancellationTokenSource.Token);

        // Get the created user resource
        var getUserResponse = await TestServerHttpClient
            .GetFromJsonAsync<GetUserResponse>(
                createUserHttpResponse.Headers.Location,
                cancellationTokenSource.Token);

        // Act
        HttpResponseMessage authenticateUserResponseMessage = await TestServerHttpClient.PostAsJsonAsync(
            requestUri: "api/auth",
            value: new AuthenticateUserRequest
            {
                Email = getUserResponse!.Email,
                Password = preHashPassword
            },
            cancellationToken: cancellationTokenSource.Token);

        // Assert
        using (var _ = new AssertionScope())
        {
            authenticateUserResponseMessage.Should().HaveStatusCode(HttpStatusCode.OK);

            var authenticateUserResponse = await authenticateUserResponseMessage
                .Content
                .ReadFromJsonAsync<AuthenticateUserResponse>(cancellationTokenSource.Token);

            authenticateUserResponse.Should().NotBeNull();
            authenticateUserResponse?.Token.Should().NotBeNullOrEmpty();
        }
    }

    [Fact(DisplayName = "Get Access Token when user was previously created but is Inactive, returns 403 Forbidden.")]
    public async Task GetAccessToken_WhenUserExistsAndIsInactive_ReturnsForbidden()
    {
        // Arrange
        CancellationTokenSource cancellationTokenSource = new();

        CreateUserRequest createUserRequest = TestUserBuilder.CreateUserRequest(UserTestData.DumpUser());
        createUserRequest.Status = Enum.GetName(UserStatus.Inactive)!;
        string preHashPassword = createUserRequest.Password;

        // Create the user
        HttpResponseMessage createUserHttpResponse = await TestServerHttpClient.PostAsJsonAsync(
            requestUri: "api/users",
            value: createUserRequest,
            cancellationToken: cancellationTokenSource.Token);

        // Get the created user resource
        HttpResponseMessage getUserHttpResponse = await TestServerHttpClient.GetAsync(createUserHttpResponse.Headers.Location, cancellationTokenSource.Token);
        var getUserResponse = await getUserHttpResponse.Content.ReadFromJsonAsync<GetUserResponse>(cancellationTokenSource.Token);

        // Act
        HttpResponseMessage authenticateUserResponseMessage = await TestServerHttpClient.PostAsJsonAsync(
            requestUri: "api/auth",
            value: new AuthenticateUserRequest
            {
                Email = getUserResponse!.Email,
                Password = preHashPassword
            },
            cancellationToken: cancellationTokenSource.Token);

        // Assert
        using (var _ = new AssertionScope())
        {
            authenticateUserResponseMessage.Should()
                .HaveStatusCode(HttpStatusCode.Forbidden, 
                    $"the created user has a {UserStatus.Inactive} status, so it shouldn't be able to authenticate.");
        }
    }
}