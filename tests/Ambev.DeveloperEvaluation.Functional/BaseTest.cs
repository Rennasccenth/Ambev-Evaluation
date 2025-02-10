using System.Net.Http.Headers;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Functional.Features.Users;
using Ambev.DeveloperEvaluation.Functional.TestCollections;
using Ambev.DeveloperEvaluation.Functional.TestData;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.Commands.AuthenticateUser;
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

    private static HttpClient? _adminAuthenticatedHttpClient;
    protected async Task<HttpClient> AuthenticateAsAdminAsync(HttpClient? client = null)
    {
        if (_adminAuthenticatedHttpClient is not null)
        {
            return _adminAuthenticatedHttpClient;
        }
        
        const string adminUserEmail = "Admin@stubmail.com";
        const string adminUserPassword = "AdminPassword@5000";
        
        HttpClient authenticatedClient = client ?? WebApplicationFactory.CreateClient();
        HttpResponseMessage authenticateUserHttpResponse = await authenticatedClient.PostAsJsonAsync(
            requestUri: "api/auth/login",
            value: new AuthenticateUserRequest
            {
                Email = adminUserEmail,
                Password = adminUserPassword
            });
        authenticateUserHttpResponse.EnsureSuccessStatusCode();
        var authenticateUserResponse = await authenticateUserHttpResponse.Content.ReadFromJsonAsync<AuthenticateUserResponse>();
        
        authenticatedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticateUserResponse?.Token);
        _adminAuthenticatedHttpClient = authenticatedClient;
        return authenticatedClient;
    }

    private static HttpClient? _customerAuthenticatedHttpClient;
    protected async Task<HttpClient> AuthenticateAsCustomerAsync(string? email = null, string? password = null, Guid? userId = null)
    {
        if (_customerAuthenticatedHttpClient is not null)
        {
            return _customerAuthenticatedHttpClient;
        }
        
        HttpClient authenticatedClient = WebApplicationFactory.CreateClient();
        HttpResponseMessage authenticateUserHttpResponse = await authenticatedClient.PostAsJsonAsync(
            requestUri: "api/auth/login",
            value: new AuthenticateUserRequest
            {
                Email = email ?? "testing_customer@mmail.com",
                Password = password ?? "test!ng_Cust0mer"
            });
        authenticateUserHttpResponse.EnsureSuccessStatusCode();
        var authenticateUserResponse = await authenticateUserHttpResponse.Content.ReadFromJsonAsync<AuthenticateUserResponse>();
        
        authenticatedClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authenticateUserResponse?.Token}");
        _customerAuthenticatedHttpClient = authenticatedClient;
        return authenticatedClient;
    }
}