namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Strategies;

public interface ICartItemQuantityDiscountStrategy
{
    decimal GetDiscountPercentage(int quantity);
    bool ValidateUniqueItemQuantity(int quantity);
    bool ValidateTotalItemsQuantity(int quantity);
}