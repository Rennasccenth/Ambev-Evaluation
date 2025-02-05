using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;

public sealed class Sale : BaseEntity
{
    public Guid CustomerId { get; set; }
    public int Number { get; set; }
    public DateTime? CreatedDate { get; private set; }
    public DateTime? TerminationDate { get; private set; }
    public DateTime? CanceledDate { get; private set; }
    public string Branch { get; set; } = null!;
    public List<SaleProduct> Products { get; init; } = [];
    
    public bool Canceled => CanceledDate.HasValue;
    public bool Terminated => TerminationDate.HasValue;
    public decimal TotalAmount => Products.Sum(prod => prod.TotalPrice);
    
    internal Sale() { }

    private Sale(Guid customerId, string branch, DateTime createdDate)
    {
        Id = Guid.NewGuid();
        CreatedDate = createdDate;
        CustomerId = customerId;
        Branch = branch;
    }

    public Sale AddProduct(SaleProduct addingProduct, TimeProvider timeProvider)
    {
        if (Products.Any(prod => prod.ProductId == addingProduct.ProductId))
        {
            SaleProduct product = Products.First(prod => prod.ProductId == addingProduct.ProductId);
            product.IncreaseQuantity((uint)addingProduct.Quantity);
            AddDomainEvent(SaleModifiedDomainEvent.Create(this, timeProvider));
            return this;
        }

        Products.Add(addingProduct);
        return this;
    }

    public Sale Sell(TimeProvider timeProvider)
    {
        AddDomainEvent(SaleTerminatedDomainEvent.Create(this, timeProvider));
        TerminationDate = timeProvider.GetUtcNow().DateTime;
        return this;
    }

    public static Sale Create(Guid customerId, string branch, TimeProvider timeProvider)
    {
        Sale creatingSale = new Sale(customerId, branch, timeProvider.GetUtcNow().DateTime);
        creatingSale.AddDomainEvent(SaleCreatedDomainEvent.Create(creatingSale, timeProvider));
        return creatingSale;
    }

    public Sale CancelProducts(Func<SaleProduct, bool> productPredicate, TimeProvider timeProvider)
    {
        var anyProductWasCanceled = false;
        var products = Products.Where(productPredicate).ToList();
        foreach (SaleProduct product in products)
        {
            anyProductWasCanceled = true;
            product.Cancel(timeProvider);
            AddDomainEvent(ItemCanceledDomainEvent.Create(Id, product.ProductId, timeProvider));
        }

        if (anyProductWasCanceled)
        {
            AddDomainEvent(SaleModifiedDomainEvent.Create(this, timeProvider));
        }
        // if (products.Count is 0) // The predicate canceled all products! We should cancel the sale? This is a business question
        // {
        //     return Cancel(timeProvider);
        // }
        return this;
    }

    /// <summary>
    /// Cancel all <see cref="SaleProduct"/> associated with this <see cref="Sale"/> and then cancel it.
    /// </summary>
    /// <param name="timeProvider"></param>
    /// <returns>Canceled <see cref="Sale"/> </returns>
    public Sale Cancel(TimeProvider timeProvider)
    {
        CanceledDate = timeProvider.GetUtcNow().DateTime;
        CancelProducts(_ => true, timeProvider);
        AddDomainEvent(SaleCanceledDomainEvent.Create(Id, timeProvider));
        return this;
    }

    private bool UpdateBranch(string branch, TimeProvider timeProvider)
    {
        if (branch == Branch) return false;
        
        Branch = branch;
        AddDomainEvent(SaleModifiedDomainEvent.Create(this, timeProvider));
        return true;
    }
}