using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM;

internal sealed class EnsureDatabaseCreatedHostService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<EnsureDatabaseCreatedHostService> _logger;

    public EnsureDatabaseCreatedHostService(IServiceScopeFactory serviceScopeFactory,
        ILogger<EnsureDatabaseCreatedHostService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Ensuring postgreSQL database creation");

        IServiceScope serviceScope = _serviceScopeFactory.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<DefaultContext>();
        
        _logger.LogInformation("Database uses the following connectionString {ConnectionString}",
            dbContext.Database.GetConnectionString());

        return dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}