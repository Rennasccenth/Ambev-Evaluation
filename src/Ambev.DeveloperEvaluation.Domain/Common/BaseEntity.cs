using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Events;

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

    public List<IEvent> DomainEvents { get; } = [];

    protected void AddDomainEvent(IEvent @event)
    {
        DomainEvents.Add(@event);
    } 
}
