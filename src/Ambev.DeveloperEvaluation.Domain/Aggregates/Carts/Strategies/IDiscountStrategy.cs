namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Strategies;

public interface IDiscountStrategy
{
    decimal GetDiscountPercentage(int quantity);
}