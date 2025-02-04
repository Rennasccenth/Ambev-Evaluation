using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;

public sealed class Cart : BaseEntity
{
    public required Guid UserId { get; init; }
    public required DateTime Date { get; init; }
    public required List<CartProduct> Products { get; init; } = [];

    internal Cart() { }
    public Cart(Guid userId, DateTime date)
    {
        UserId = userId;
        Date = date;
    }

    public Cart(Guid userId, DateTime date, List<CartProduct> products)
    {
        UserId = userId;
        Date = date;
        Products = products;
    }

    public int CountProducts(Guid productId)
    {
        CartProduct? product = Products.FirstOrDefault(p => p.ProductId == productId);

        return product?.Quantity ?? 0;
    }

    public Cart AddProduct(Guid productId, int quantity)
    {
        CartProduct? product = Products.FirstOrDefault(p => p.ProductId == productId);
        if (product is not null)
        {
            product.IncreaseQuantity(quantity);
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