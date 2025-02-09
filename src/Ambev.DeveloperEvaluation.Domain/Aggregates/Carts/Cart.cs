using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;

public sealed class Cart : BaseEntity
{
    public Guid? UserId { get; init; }
    public DateTime Date { get; init; }
    public List<CartProduct> Products { get; init; } = [];

    internal Cart() { }

    /// <summary>
    /// Creates a Empty cart without associating to a User
    /// </summary>
    public Cart(DateTime date)
    {
        Id = Guid.NewGuid();
        Date = date;
    }

    /// <summary>
    /// Creates a Non-Empty cart without associating to a User
    /// </summary>
    public Cart(DateTime date, IEnumerable<CartProduct> products)
    {
        Id = Guid.NewGuid();
        Date = date;
        Products = products.ToList();
    }
    
    /// <summary>
    /// Creates a Empty Cart associated to a User
    /// </summary>
    public Cart(Guid userId, DateTime date)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Date = date;
    }

    /// <summary>
    /// Creates a Non-Empty Cart associated to a User
    /// </summary>
    public Cart(Guid userId, DateTime date, List<CartProduct> products)
    {
        Id = Guid.NewGuid();
        UserId = userId;
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