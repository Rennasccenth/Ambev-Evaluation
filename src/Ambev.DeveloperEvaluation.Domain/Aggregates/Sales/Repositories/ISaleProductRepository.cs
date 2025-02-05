namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Repositories;

public interface ISaleProductRepository
{
    Task<IEnumerable<SaleProduct>> GetSaleProductsAsync(IEnumerable<Guid> saleProductIds, CancellationToken ct);
    
}