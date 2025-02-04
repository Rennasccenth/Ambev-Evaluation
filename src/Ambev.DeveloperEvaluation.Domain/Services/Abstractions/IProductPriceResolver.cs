namespace Ambev.DeveloperEvaluation.Domain.Services.Abstractions;

public interface IProductPriceResolver
{
    Task<Dictionary<Guid, decimal>> ResolveProductsUnitPriceAsync(IEnumerable<Guid> productsId, CancellationToken ct);
}