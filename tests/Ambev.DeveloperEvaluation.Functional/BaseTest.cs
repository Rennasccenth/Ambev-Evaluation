using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Functional.Features.Users;
using Ambev.DeveloperEvaluation.Functional.TestCollections;
using Ambev.DeveloperEvaluation.Functional.TestData;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional;

[Collection(PerCollectionFixture.CollectionName)]
public class BaseTest : IAsyncLifetime
{
    /// <summary>
    /// Configured <see cref="HttpClient"/> that points to current running Test Server.
    /// </summary>
    protected readonly HttpClient TestServerHttpClient;
    protected readonly FakeTimeProvider CurrentTimeProvider;
    protected readonly DeveloperEvaluationWebApplicationFactory WebApplicationFactory;
    private readonly Func<Task> _restartDatabaseStateAsync;

    protected BaseTest(DeveloperEvaluationWebApplicationFactory webApplicationFactory)
    {
        _restartDatabaseStateAsync = webApplicationFactory.ResetDatabasesAsync;
        TestServerHttpClient = webApplicationFactory.CreateClient();
        WebApplicationFactory = webApplicationFactory;
        IServiceScope serviceScope = webApplicationFactory.Services.CreateScope();
        CurrentTimeProvider = (FakeTimeProvider) serviceScope.ServiceProvider.GetRequiredService<TimeProvider>();
        UserTestData = new UserTestData(webApplicationFactory);
        ProductTestData = new ProductsTestData(webApplicationFactory);
    }

    // Nothing needed for now, but can be used for seed the database 
    public Task InitializeAsync() => Task.CompletedTask;

    // Restart the database by calling configured respawn restart function, this is called after each test.
    public Task DisposeAsync() => _restartDatabaseStateAsync();

    // Test data initialization
    protected readonly UserTestData UserTestData;
    protected readonly ProductsTestData ProductTestData;

    protected async Task AuthenticateAsync(string email, string password, Guid? userId = null)
    {
        HttpResponseMessage authenticateUserHttpResponse = await TestServerHttpClient.PostAsJsonAsync(
            requestUri: "api/auth",
            value: new AuthenticateUserRequest
            {
                Email = email,
                Password = password
            });
        authenticateUserHttpResponse.EnsureSuccessStatusCode();
        var authenticateUserResponse = await authenticateUserHttpResponse.Content.ReadFromJsonAsync<AuthenticateUserResponse>();
        
        TestServerHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authenticateUserResponse?.Token}");
        TestServerHttpClient.DefaultRequestHeaders.Add("UserId", userId.ToString());
    }

    /// <summary>
    /// Acts as a Authenticated User.
    /// </summary>
    protected async Task ActAsAuthenticatedUserAsync()
    {
        if (TestServerHttpClient.DefaultRequestHeaders.Contains("Authorization")) return;
        
        var authenticatingUser = UserTestData.DumpUser(status: UserStatus.Active);
        string email = authenticatingUser.Email;
        string password = authenticatingUser.Password;
        
        HttpResponseMessage createUserHttpResponse = await TestServerHttpClient.PostAsJsonAsync(
            requestUri: "api/users",
            value: TestUserBuilder.CreateUserRequest(authenticatingUser));
        createUserHttpResponse.EnsureSuccessStatusCode();

        var createUserResponse = await createUserHttpResponse.Content.ReadFromJsonAsync<CreateUserResponse>();
        await AuthenticateAsync(email, password, createUserResponse?.Id);
    }
}