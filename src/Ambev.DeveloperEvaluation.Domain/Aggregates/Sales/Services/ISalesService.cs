using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;

public interface ISalesService
{
    Task<ApplicationResult<Sale>> CreateSaleAsync(Cart cart, string branch, IProductPriceResolver productPriceResolver,
        ISpecification<Cart>? specification = null, CancellationToken ct = default);
    Task<ApplicationResult<Sale>> CancelSaleAsync(Guid saleId, CancellationToken ct);
    Task<ApplicationResult<Sale>> ConcludeSaleAsync(Guid saleId, CancellationToken ct);
}
