using System.Data.Common;
using Ambev.DeveloperEvaluation.MongoDB;
using Ambev.DeveloperEvaluation.PostgreSQL;
using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using MongoDB.Driver;
using Respawn;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional;

/// <summary>
/// <inheritdoc cref="WebApplicationFactory{TEntry}" />
/// <br/>
/// Runs the WebAPI as a Test Server. Executing the real code as they are.
/// <br/>
/// Important Notes:
/// <ul>
///     <li> We are replacing any configured PostgreSQL instance for a containerized instance that is disposed after finishing the test suite. </li>
///     <li> Any implementation that calls external dependencies should be carefully reviewed, because real HttpRequests will be sent. In such cases, I do suggest Mocking external calls or point them to Sandbox environments, if available. </li>
/// </ul> 
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class DeveloperEvaluationWebApplicationFactory
    : WebApplicationFactory<Program>,
        IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase("DeveloperEvaluation")
        .WithUsername("DeveloperEvaluationApplication")
        .WithPassword("YourStrong@Passw0rd")
        .Build();

    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithUsername("DeveloperEvaluationApplication")
        .WithPassword("YourStrong@Passw0rd")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();

    private Respawner _respawner = null!; 
    private DbConnection _postgreSqlDbConnection = null!;
    private IMongoClient _mongoDbClient = null!;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(collection =>
        {
            collection
                .RemoveAll<TimeProvider>()
                .AddSingleton<TimeProvider>(new FakeTimeProvider(DateTimeOffset.UtcNow));

            // Replace the current Context by pointing it to a PostgreSQL Container instance.
            collection
                .RemoveAll<DbContextOptions<DefaultContext>>()
                .AddDbContext<DefaultContext>(optionsBuilder =>
                {
                    optionsBuilder.UseNpgsql(_postgreSqlContainer.GetConnectionString(), psqlOptions =>
                    {
                        psqlOptions.CommandTimeout(3);
                        psqlOptions.EnableRetryOnFailure(2);
                    });
                    optionsBuilder.EnableDetailedErrors();
                    optionsBuilder.EnableSensitiveDataLogging();
                }, contextLifetime: ServiceLifetime.Transient);

            // Remove actual client and points mongo to test container instance
            collection
                .RemoveAll<IMongoClient>()
                .AddSingleton<IMongoClient>(_ => new MongoClient(_mongoDbContainer.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        List<Task> containerSpinUpTasks = [
            _postgreSqlContainer.StartAsync(),
            _mongoDbContainer.StartAsync()
        ];

        await Task.WhenAll(containerSpinUpTasks);
        
        await StartPostgreSqlRespawner();
    }

    /// <summary>
    /// This method is called by Respawner, to ensure the databases are in a **known state** before starting a Test.
    /// </summary>
    public async Task ResetDatabasesAsync()
    {
        List<Task> databaseResetTasks = [
            ResetRespawnerDatabasesAsync(),
            ResetMongoDbDatabaseAsync()
        ];

        await Task.WhenAll(databaseResetTasks);
    }
    
    public new async Task DisposeAsync()
    {
        List<ValueTask> containerDisposalValueTasks = 
        [
            _postgreSqlContainer.DisposeAsync(),
            _mongoDbContainer.DisposeAsync()
        ];

        await Task.WhenAll(containerDisposalValueTasks.Select(valueTask => valueTask.AsTask()));
    }

    private Task ResetMongoDbDatabaseAsync()
    {
        IServiceScope scope = Services.CreateScope();
        _mongoDbClient = scope.ServiceProvider
            .GetRequiredService<IMongoClient>();

        MongoDbSettings mongoDbSettings = scope.ServiceProvider
            .GetRequiredService<IOptions<MongoDbSettings>>().Value;

        return _mongoDbClient.DropDatabaseAsync(mongoDbSettings.DatabaseName);
    }

    /// <summary>
    /// Reset all relational databases monitored by respawner, which is only PostgreSQL in this case.
    /// </summary>
    private async Task ResetRespawnerDatabasesAsync()
    {
        await _respawner.ResetAsync(_postgreSqlDbConnection);
        IServiceScope scope = Services.CreateScope();
        DefaultContext dbContext = scope.ServiceProvider.GetRequiredService<DefaultContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    /// <summary>
    /// Start respawner monitoring over configured relational databases.
    /// </summary>
    private async Task StartPostgreSqlRespawner()
    {
        IServiceScope scope = Services.CreateScope();
        DefaultContext dbContext = scope.ServiceProvider.GetRequiredService<DefaultContext>();
        _postgreSqlDbConnection = dbContext.Database.GetDbConnection();

        await EnsureAllTablesExists(dbContext);
        await InitializeRespawnerAsync(_postgreSqlDbConnection);
    }

    /// <summary>
    /// Ensure Entity Framework created all mapped tables.
    /// </summary>
    private async Task EnsureAllTablesExists(DbContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();
    }

    /// <summary>
    /// Start Respawner monitoring over a single DbConnection.
    /// </summary>
    private async Task InitializeRespawnerAsync(DbConnection dbConnection)
    {
        await dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }
}