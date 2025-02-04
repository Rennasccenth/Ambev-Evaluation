using Ambev.DeveloperEvaluation.Functional.TestCollections;
using Ambev.DeveloperEvaluation.Functional.TestData;
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
}