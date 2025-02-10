using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;

public sealed class SaleProduct
{
    public Guid SaleId { get; }
    public Guid ProductId { get; init; }
    public decimal UnitPrice { get; init; }
    public int Quantity { get; private set; }
    public decimal Discounts { get; set; }
    public bool IsCanceled => CanceledAt is not null;
    public DateTime? CanceledAt { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity - Discounts;
    // public decimal TotalDiscounts => Discounts * Quantity;

    public SaleProduct(Guid saleId, Guid productId, decimal unitPrice, int quantity, decimal discounts)
    {
        SaleId = saleId;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Discounts = discounts;
    }

    public SaleProduct IncreaseQuantity(uint quantity)
    {
        Quantity += (int)quantity;
        return this;
    }
    
    public SaleProduct DecreaseQuantity(uint quantity)
    {
        Quantity -= (int)quantity;
        if (Quantity < 0)
        {
            Quantity = 0;
        }
        return this;
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