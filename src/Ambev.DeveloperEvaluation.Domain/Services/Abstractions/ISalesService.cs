using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Services.Abstractions;

public interface ISalesService
{
    Task<Sale> CreateSaleAsync(Cart cart, string branch, IProductPriceResolver productPriceResolver, CancellationToken ct);
    // Task<Sale> UpdateSaleAsync(Cart cart, IProductPriceResolver productPriceResolver, CancellationToken ct);
    Task<Sale?> CancelSaleAsync(Guid saleId, CancellationToken ct);
    Task<Sale?> ConcludeSaleAsync(Guid saleId, CancellationToken ct);
}
