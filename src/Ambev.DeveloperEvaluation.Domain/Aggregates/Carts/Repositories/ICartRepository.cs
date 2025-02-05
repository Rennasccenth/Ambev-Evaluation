using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;

public interface ICartRepository
{
    Task<Cart?> FindByUserIdAsync(Guid id, CancellationToken ct);
    Task<PaginatedList<Cart>> GetByFilterAsync(GetCartsQueryFilter queryFilter, CancellationToken ct);
    Task<bool> DeleteAsync(Guid cartId, CancellationToken ct);
    Task<Cart> UpsertAsync(Cart updatingCart, CancellationToken ct);
}