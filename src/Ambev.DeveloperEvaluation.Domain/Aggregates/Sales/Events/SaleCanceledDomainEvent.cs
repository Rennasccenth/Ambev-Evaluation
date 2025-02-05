using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Events;

public sealed class SaleCanceledDomainEvent : IEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }
    public Guid SaleId { get; }

    private SaleCanceledDomainEvent(Guid saleId, TimeProvider timeProvider)
    {
        Id = Guid.NewGuid();
        SaleId = saleId;
        DateOccurred = timeProvider.GetUtcNow().DateTime;
    }

    public static SaleCanceledDomainEvent Create(Guid saleId, TimeProvider timeProvider)
    {
        return new SaleCanceledDomainEvent(saleId, timeProvider);
    }
}