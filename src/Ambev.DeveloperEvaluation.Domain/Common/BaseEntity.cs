using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Events.Abstractions;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public class BaseEntity : IComparable<BaseEntity>, IEventableEntity
{
    public Guid Id { get; set; }

    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }

    public int CompareTo(BaseEntity? other)
    {
        if (other == null)
        {
            return 1;
        }

        return other.Id.CompareTo(Id);
    }
    
    public IReadOnlyList<IEvent> DomainEvents => [.._domainEvents.Values];

    private readonly Dictionary<Guid, IEvent> _domainEvents = new();

    public void AddDomainEvent(IEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));
        _domainEvents.TryAdd(domainEvent.Id, domainEvent);
    }

    public void RemoveDomainEvent(Guid eventId)
    {
        _domainEvents.Remove(eventId);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
