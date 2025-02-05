using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Events;

public sealed class SaleCanceledEvent : IEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }
    public Guid SaleId { get; }

    private SaleCanceledEvent(Guid saleId, TimeProvider timeProvider)
    {
        Id = Guid.NewGuid();
        SaleId = saleId;
        DateOccurred = timeProvider.GetUtcNow().DateTime;
    }

    public static SaleCanceledEvent Create(Guid saleId, TimeProvider timeProvider)
    {
        return new SaleCanceledEvent(saleId, timeProvider);
    }
}