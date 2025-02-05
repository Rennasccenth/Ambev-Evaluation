namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Strategies;

internal class ItemQuantity : IDiscountStrategy
{
    public decimal GetDiscountPercentage(int quantity) =>
        quantity switch
        {
            >= 10 and <= 20 => 0.20m,
            >= 4 => 0.10m,
            _ => 0m
        };

    public bool ValidateUniqueItemQuantity(int quantity) => quantity is < 20 and > 1;
    public bool ValidateTotalItemsQuantity(int quantity) => true;
}