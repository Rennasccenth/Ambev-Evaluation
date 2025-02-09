using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;

public interface ICartRepository
{
    Task<PaginatedList<Cart>> GetByFilterAsync(GetCartsQueryFilter queryFilter, CancellationToken ct);

    Task<Cart?> FindByUserIdAsync(Guid userId, CancellationToken ct);
    Task<Cart?> FindByCartIdAsync(Guid cartId, CancellationToken ct);
    Task<bool> DeleteAsync(Guid cartId, CancellationToken ct);
    Task<Cart> UpsertAsync(Cart updatingCart, CancellationToken ct);
    Task<Cart> CreateAsync(Cart cart, CancellationToken ct = default);
}