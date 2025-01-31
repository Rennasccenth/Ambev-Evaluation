using Xunit;

namespace Ambev.DeveloperEvaluation.Functional;

[Collection(TestHelperConstants.WebApiCollectionName)]
public class BaseTest : IAsyncLifetime
{
    /// <summary>
    /// Configured <see cref="HttpClient"/> that points to current running Test Server.
    /// </summary>
    protected HttpClient TestServerHttpClient { get; init; }
    
    /// <summary>
    /// Currently configured <see cref="IServiceProvider"/> for the running Test Server. 
    /// </summary>
    protected IServiceProvider TestServerServiceProvider { get; init; }

    private readonly Func<Task> _restartDatabaseStateAsync;

    protected BaseTest(DeveloperEvaluationWebApplicationFactory webApplicationFactory)
    {
        _restartDatabaseStateAsync = webApplicationFactory.ResetDatabaseAsync;
        TestServerHttpClient = webApplicationFactory.CreateClient();
        TestServerServiceProvider = webApplicationFactory.Services;
    }

    // Nothing needed for now, but can be used for seed the database 
    public Task InitializeAsync() => Task.CompletedTask;

    // Restart the database by calling configured respawn restart function.
    public Task DisposeAsync() => _restartDatabaseStateAsync();
}