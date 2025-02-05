using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;

public sealed class Cart : BaseEntity
{
    public Guid CustomerId { get; init; }
    public DateTime Date { get; init; }
    public List<CartProduct> Products { get; init; } = [];

    internal Cart() { }

    public Cart(Guid userId, DateTime date)
    {
        Id = Guid.NewGuid();
        CustomerId = userId;
        Date = date;
    }

    public Cart(Guid userId, DateTime date, List<CartProduct> products)
    {
        Id = Guid.NewGuid();
        CustomerId = userId;
        Date = date;
        Products = products;
    }

    public int CountProducts(Func<CartProduct, bool> predicate) => Products.Count(predicate);

    public Cart UpsertProduct(Guid productId, int quantity)
    {
        CartProduct? product = Products.FirstOrDefault(p => p.ProductId == productId);
        if (product is not null)
        {
            product.OverrideQuantity(quantity);
            return this;
        }

        Products.Add(new CartProduct(productId, quantity));
        return this;
    }

    public Cart RemoveProduct(Guid productId, int quantity)
    {
        CartProduct? product = Products.FirstOrDefault(p => p.ProductId == productId);
        if (product is null) return this;

        product.DecreaseQuantity(quantity);
        if (product.Quantity <= 0)
            Products.Remove(product);
        return this;
    }

    public Cart Empty()
    {
        Products.Clear();
        return this;
    }
}