using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Products.Repositories;

public interface IProductRegistryRepository
{
    Task<Product?> FindByIdAsync(Guid id, CancellationToken ct);
    Task<List<Product>> GetAllByIds(IEnumerable<Guid> productsId, CancellationToken ct);
    Task<PaginatedList<Product>> GetByFilterAsync(GetRegisteredProductsQueryFilter queryFilter, CancellationToken ct);
    ValueTask<bool> ExistsAllAsync(IEnumerable<Guid> productIds, CancellationToken ct);
    Task<Product> CreateAsync(Product creatingProduct, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    Task<Product> UpdateAsync(Product updatingProduct, CancellationToken ct);
    Task<List<string>> GetCategoriesAsync(CancellationToken ct);
}
