using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Events.Users;

public sealed record UserRegisteredEvent : IEvent
{
    public Guid Id { get; }
    public Email Email { get; }
    public string Firstname { get; }
    public string Lastname { get; }
    public DateTime DateOccurred { get; }

    private UserRegisteredEvent(Email email, string firstname, string lastname, DateTime dateOccurred)
    {
        Email = email;
        Firstname = firstname;
        Lastname = lastname;
        DateOccurred = dateOccurred;
        Id = Guid.NewGuid();
    }
    
    public static UserRegisteredEvent From(User user, TimeProvider timeProvider)
    {
        return new UserRegisteredEvent(user.Email, user.Firstname, user.Lastname, timeProvider.GetUtcNow().UtcDateTime);
    }
}