using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Repositories;

public interface ISaleRepository
{
    Task<Sale?> FindByIdAsync(Guid id, CancellationToken ct);
    Task<PaginatedList<Sale>> GetByFilterAsync(GetSalesQueryFilter queryFilter, CancellationToken ct);
    Task<Sale> CreateAsync(Sale creatingSale, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    Task<Sale> UpdateAsync(Sale updatingSale, CancellationToken ct);
}