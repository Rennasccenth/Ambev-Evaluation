using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Events;

public sealed class SaleCreatedEvent : IEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }
    public Guid SaleId { get; }
    public IReadOnlyList<SaleProduct> Products { get; }
    public decimal TotalAmount { get; }

    private SaleCreatedEvent(Sale sale, TimeProvider timeProvider)
    {
        Id = Guid.NewGuid();
        SaleId = sale.Id;
        DateOccurred = timeProvider.GetUtcNow().DateTime;
        Products = sale.Products;
        TotalAmount = sale.TotalAmount;
    }
    
    public static SaleCreatedEvent Create(Sale sale, TimeProvider timeProvider) => new(sale, timeProvider);
}