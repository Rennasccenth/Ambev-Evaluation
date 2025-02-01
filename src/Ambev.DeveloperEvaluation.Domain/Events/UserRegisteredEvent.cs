namespace Ambev.DeveloperEvaluation.Domain.Events;

public sealed record UserRegisteredEvent : IEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }
    public Guid UserId { get; }

    public UserRegisteredEvent(Guid userId, DateTime dateOccurred)
    {
        UserId = userId;
        DateOccurred = dateOccurred;
        Id = Guid.NewGuid();
    }
}