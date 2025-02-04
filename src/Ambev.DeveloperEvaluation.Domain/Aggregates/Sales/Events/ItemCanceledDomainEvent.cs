using Ambev.DeveloperEvaluation.Domain.Events.Abstractions;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Events;

public sealed class ItemCanceledDomainEvent : IEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }
    public Guid SaleId { get; }
    public Guid ProductId { get; }

    private ItemCanceledDomainEvent(Guid saleId, Guid productId, TimeProvider timeProvider)
    {
        Id = Guid.NewGuid();
        SaleId = saleId;
        ProductId = productId;
        DateOccurred = timeProvider.GetUtcNow().UtcDateTime;
    }

    public static ItemCanceledDomainEvent Create(Guid saleId, Guid productId, TimeProvider timeProvider)
    {
        return new ItemCanceledDomainEvent(saleId, productId, timeProvider);
    }
}