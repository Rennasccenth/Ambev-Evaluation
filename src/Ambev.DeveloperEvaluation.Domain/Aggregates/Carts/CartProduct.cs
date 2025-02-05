namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;

public sealed class CartProduct
{
    public Guid ProductId { get; init; }
    public int Quantity { get; private set; }

    internal CartProduct() { }

    public CartProduct(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    public CartProduct IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
        return this;
    }

    public CartProduct OverrideQuantity(int quantity)
    {
        Quantity += quantity;
        return this;
    }

    public CartProduct DecreaseQuantity(int quantity)
    {
        Quantity -= quantity;
        return this;
    }
}