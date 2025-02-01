namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Marker interface for Domain Events.
/// </summary>
public interface IEvent
{
    /// <summary>
    /// Gets the event identifier.
    /// </summary>
    Guid Id { get; }
    
    /// <summary>
    /// Gets the date and time when the event occurred.
    /// </summary>
    DateTime DateOccurred { get; }
}