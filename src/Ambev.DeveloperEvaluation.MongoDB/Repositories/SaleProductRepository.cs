using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Repositories;

namespace Ambev.DeveloperEvaluation.MongoDB.Repositories;

public sealed class SaleProductRepository : ISaleProductRepository
{
    public Task<IEnumerable<SaleProduct>> GetSaleProductsAsync(IEnumerable<Guid> saleProductIds, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}