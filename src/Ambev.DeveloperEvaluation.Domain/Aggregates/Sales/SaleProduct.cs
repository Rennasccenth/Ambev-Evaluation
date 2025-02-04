using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;

public sealed class SaleProduct
{
    // public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Discounts { get; private set; }

    public bool IsCanceled => CanceledAt is null;
    public DateTime? CanceledAt { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity - Discounts;
    // public decimal TotalDiscounts => Discounts * Quantity;

    public SaleProduct(Guid productId, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
        // Id = Guid.NewGuid();
    }

    public void Cancel(TimeProvider timeProvider)
    {
        CanceledAt = timeProvider.GetUtcNow().DateTime;
    }

    public void ApplyPercentDiscount(decimal discount)
    {
        if (discount is < 0 or > 100)
        {
            throw new InvalidDiscountException("Discount cannot be greater than 100%");
        }
        var percentDiscountValue = UnitPrice * Quantity * (discount / 100);
        Discounts = percentDiscountValue;
    }
    public void ApplyFixedDiscount(decimal discount)
    {
        if (discount < 0)
        {
            throw new InvalidDiscountException("Discount cannot be negative");
        }
        Discounts = discount;
    }
    public void ApplyNoDiscount()
    {
        Discounts = 0;
    }
    public SaleProduct ResetDiscount()
    {
        Discounts = 0;
        return this;
    }
}