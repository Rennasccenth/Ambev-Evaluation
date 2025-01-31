using System.Data.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using DotNet.Testcontainers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Respawn;
using Serilog;
using Testcontainers.PostgreSql;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using ILogger = Microsoft.Extensions.Logging.ILogger;

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

    private Respawner _respawner = null!; 
    private DbConnection _dbConnection = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(collection =>
        {
            collection
                .RemoveAll<TimeProvider>()
                .AddSingleton<TimeProvider>(new FakeTimeProvider(DateTimeOffset.UtcNow));
            
            // Replace the current Context by pointing it to a PostgreSQL Container instance.
            collection.RemoveAll<DbContextOptions<DefaultContext>>()
                .AddDbContext<DefaultContext>(optionsBuilder =>
                {
                    optionsBuilder.UseNpgsql(_postgreSqlContainer.GetConnectionString(), psqlOptions =>
                    {
                        psqlOptions.CommandTimeout(3);
                        psqlOptions.EnableRetryOnFailure(2);
                    });
                });
        });
    }

    /// <summary>
    /// This method is called by Respawner, to ensure the database is in a **known state** before starting a Test.
    /// </summary>
    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
        IServiceScope scope = Services.CreateScope();
        DefaultContext dbContext = scope.ServiceProvider.GetRequiredService<DefaultContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        IServiceScope scope = Services.CreateScope();
        DefaultContext dbContext = scope.ServiceProvider
            .GetRequiredService<DefaultContext>();
        _dbConnection = dbContext.Database.GetDbConnection();

        await RunMigrationsOnDbContextAsync(dbContext);
        await InitializeRespawnerAsync(_dbConnection);
    }

    private async Task RunMigrationsOnDbContextAsync(DefaultContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();
    }

    private async Task InitializeRespawnerAsync(DbConnection dbConnection)
    {
        await dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }

    public new async Task DisposeAsync() => await _postgreSqlContainer.DisposeAsync();
}