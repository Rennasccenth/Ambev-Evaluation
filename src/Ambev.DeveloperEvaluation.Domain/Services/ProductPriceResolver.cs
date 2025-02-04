using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services.Abstractions;

namespace Ambev.DeveloperEvaluation.Domain.Services;

internal sealed class ProductPriceResolver : IProductPriceResolver
{
    private readonly ISaleProductRepository _saleProductRepository;

    public ProductPriceResolver(ISaleProductRepository saleProductRepository)
    {
        _saleProductRepository = saleProductRepository;
    }

    public async Task<Dictionary<Guid, decimal>> ResolveProductsUnitPriceAsync(IEnumerable<Guid> productsId, CancellationToken ct)
    {
        var saleProducts = await _saleProductRepository.GetSaleProductsAsync(productsId, ct);
        var enumeratedProducts = saleProducts.ToList();
        return enumeratedProducts.ToDictionary(x => x.ProductId, x => x.UnitPrice);
    }
}