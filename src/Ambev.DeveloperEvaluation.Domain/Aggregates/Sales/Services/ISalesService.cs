using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;

public interface ISalesService
{
    Task<Sale> CreateSaleAsync(Cart cart, string branch, IProductPriceResolver productPriceResolver,
        ISpecification<Cart>? specification = null, CancellationToken ct = default);
    // Task<Sale> UpdateSaleAsync(Cart cart, IProductPriceResolver productPriceResolver, CancellationToken ct);
    Task<Sale?> CancelSaleAsync(Guid saleId, CancellationToken ct);
    Task<Sale?> ConcludeSaleAsync(Guid saleId, CancellationToken ct);
}
