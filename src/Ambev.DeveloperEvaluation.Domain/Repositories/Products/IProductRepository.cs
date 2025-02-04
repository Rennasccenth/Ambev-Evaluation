using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.Products;

public interface IProductRepository
{
    Task<Product?> FindByIdAsync(Guid id, CancellationToken ct);
    Task<PaginatedList<Product>> GetByFilterAsync(GetProductsQueryFilter queryFilter, CancellationToken ct);
    Task<Product> CreateAsync(Product creatingProduct, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    Task<Product> UpdateAsync(Product updatingProduct, CancellationToken ct);
    Task<List<string>> GetCategoriesAsync(CancellationToken ct);
}
