using Ambev.DeveloperEvaluation.Domain.Events.Abstractions;
using Ambev.DeveloperEvaluation.Domain.Events.Users;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.Events;

public sealed class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
{
    private readonly ILogger<UserRegisteredEventHandler> _logger;

    public UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger)
    {
        _logger = logger;
    }
    
    public Task HandleAsync(UserRegisteredEvent @event)
    {
        _logger.LogInformation("User {Firstname} {Lastname} registered with Email {Email}", @event.Firstname, @event.Lastname, @event.Email);
        return Task.CompletedTask;
    }
}