using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.MongoDB.Repositories;

public sealed class SaleRepository : ISaleRepository
{
    public Task<Sale?> FindByIdAsync(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedList<Sale>> GetByFilterAsync(GetSalesQueryFilter queryFilter, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Sale> CreateAsync(Sale creatingSale, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Sale> UpdateAsync(Sale updatingSale, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}