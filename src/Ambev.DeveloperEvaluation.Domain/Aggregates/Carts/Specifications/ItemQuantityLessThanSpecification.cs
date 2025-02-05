using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Specifications;

public sealed class ItemQuantityLessThanSpecification : ISpecification<Cart>
{
    private const uint QuantityThreshold = 20;
    
    public bool IsSatisfiedBy(Cart entity)
    {
        return entity.Products.All(p => p.Quantity < QuantityThreshold);
    }

    public string ErrorMessage => $"Item quantity cannot be less than {QuantityThreshold}";
}