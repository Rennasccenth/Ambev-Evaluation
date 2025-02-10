using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.PostgreSQL;

/// <summary>
/// Creates an Admin <see cref="User"/> intended to be the initial user
/// that allow us to create other resources in the system.
/// </summary>
internal sealed class EnsureBootstrapAdminUserIsCreated : IHostedService
{
    private readonly DefaultContext _dbContext;
    private readonly ILogger<EnsureBootstrapAdminUserIsCreated> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly TimeProvider _timeProvider;
    private readonly IValidator<User> _userValidator;

    public EnsureBootstrapAdminUserIsCreated(
        IServiceProvider serviceProvider,
        ILogger<EnsureBootstrapAdminUserIsCreated> logger)
    {
        IServiceScope serviceScope = serviceProvider.CreateScope();
        _passwordHasher = serviceScope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        _timeProvider = serviceScope.ServiceProvider.GetRequiredService<TimeProvider>();
        _userValidator = serviceScope.ServiceProvider.GetRequiredService<IValidator<User>>();
        _dbContext = serviceScope.ServiceProvider.GetRequiredService<DefaultContext>();
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Ensuring bootstrap admin user is created.");

        try
        {
            User.UserBuilder userBuilder = User.GetBuilder(_passwordHasher, _timeProvider, _userValidator);

            Address fakeAddress = new("NewYork", "St. Chapel", 10, "6670278", "35.22532", "45.32739");
            var buildResult = userBuilder
                .WithUsername("AdminSeededUsername")
                .WithEmail("Admin@stubmail.com")
                .WithPassword("AdminPassword@5000")
                .WithAddress(fakeAddress)
                .WithFirstname("Admin")
                .WithLastname("Seeded")
                .WithPhone("123456789")
                .WithStatus(UserStatus.Active)
                .WithRole(UserRole.Admin)
                .Build();
        
            User firstAdminUser = buildResult.IsT0 ? buildResult.AsT0 : throw new Exception("Failed to create bootstrap admin user");

            var adminUserAlreadyAdded = await _dbContext.Users
                .AnyAsync(usr => usr.Email == firstAdminUser.Email, cancellationToken: cancellationToken);

            if (!adminUserAlreadyAdded)
            {
                await _dbContext.Users
                    .AddAsync(firstAdminUser, cancellationToken: cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Bootstrap admin user created.");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to bootstrap admin user.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}