using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events.Abstractions;

namespace Ambev.DeveloperEvaluation.Domain.Events.Products;

public sealed class ProductPriceChangedEvent : IEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }
    public decimal PreviousPrice { get; }
    public decimal NewPrice { get; }

    private ProductPriceChangedEvent(DateTime dateOccurred, Guid productId, decimal previousPrice, decimal newPrice)
    {
        Id = Guid.NewGuid();
        DateOccurred = dateOccurred;
        PreviousPrice = previousPrice;
        NewPrice = newPrice;
    }

    public static ProductPriceChangedEvent CreateFrom(Product product, decimal newPrice, TimeProvider timeProvider)
    {
        return new ProductPriceChangedEvent(
            dateOccurred: timeProvider.GetUtcNow().UtcDateTime, 
            productId: product.Id,
            previousPrice: product.Price,
            newPrice: newPrice);
    }
}