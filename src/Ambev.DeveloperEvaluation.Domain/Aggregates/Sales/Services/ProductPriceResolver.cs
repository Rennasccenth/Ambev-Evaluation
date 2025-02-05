using Ambev.DeveloperEvaluation.Domain.Aggregates.Products.Repositories;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;

internal sealed class ProductPriceResolver : IProductPriceResolver
{
    private readonly IProductRegistryRepository _productRegistryRepository;

    public ProductPriceResolver(IProductRegistryRepository productRegistryRepository)
    {
        _productRegistryRepository = productRegistryRepository;
    }

    public async Task<Dictionary<Guid, decimal>> ResolveProductsUnitPriceAsync(IEnumerable<Guid> productsId, CancellationToken ct)
    {
        var products = await _productRegistryRepository.GetAllByIds(productsId, ct);
        var dictionary = products
            .Select(p => new {p.Id, p.Price})
            .ToDictionary(x => x.Id, x => x.Price);

        return dictionary;
    }
}