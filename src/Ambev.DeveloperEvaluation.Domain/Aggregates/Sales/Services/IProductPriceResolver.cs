namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;

public interface IProductPriceResolver
{
    Task<Dictionary<Guid, decimal>> ResolveProductsUnitPriceAsync(IEnumerable<Guid> productsId, CancellationToken ct);
}