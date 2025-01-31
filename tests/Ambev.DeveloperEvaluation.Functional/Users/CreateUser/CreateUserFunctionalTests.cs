using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using DotNet.Testcontainers;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Users.CreateUser;

public sealed class CreateUserFunctionalTests : BaseFunctionalTest
{
    public CreateUserFunctionalTests(DeveloperEvaluationWebApplicationFactory webApplicationFactory) 
        : base(webApplicationFactory) { }

    [Fact(DisplayName = "Get user when it exists should return 200 Ok containing the user response.")]
    public async Task GetUserById_WhenUserExists_ReturnsOkWithUser()
    {
        // Arrange
        CancellationTokenSource cancellationTokenSource = new();

        HttpResponseMessage createUserHttpResponse = await TestServerHttpClient.PostAsJsonAsync(
            requestUri: "api/Users",
            value: UserBuilder.GetValidUserRequest(),
            cancellationToken: cancellationTokenSource.Token);

        // Act
        HttpResponseMessage getUserHttpResponse = await TestServerHttpClient
            .GetAsync(createUserHttpResponse.Headers.Location, cancellationTokenSource.Token);        

        // Assert
        using (var _ = new AssertionScope())
        {
            getUserHttpResponse.StatusCode.Should()
                .Be(HttpStatusCode.OK, "Once user was created, we should be able to retrieve it by calling this endpoint.");

            var getUserResponse = await getUserHttpResponse.Content.ReadFromJsonAsync<GetUserResponse>(cancellationTokenSource.Token);
            getUserResponse.Should()
                .NotBeNull("we should be able to correctly parse the response.");
        }
    }

    [Fact(DisplayName = "Get user when it doesn't exists should return 404 NotFound.")]
    public async Task GetUserById_WhenUserDoesntExists_ReturnsNotFound()
    {
        // Arrange
        CancellationTokenSource cancellationTokenSource = new();

        // Act
        HttpResponseMessage response = await TestServerHttpClient.GetAsync($"api/Users/{Guid.NewGuid()}", cancellationTokenSource.Token);        

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}