using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;
using Ambev.DeveloperEvaluation.Domain.Common;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.MongoDB.Repositories;

public sealed class CartRepository : ICartRepository
{
    private readonly IMongoCollection<Cart> _cartCollection;

    public CartRepository(IMongoCollection<Cart> cartCollection)
    {
        _cartCollection = cartCollection;
    }

    public async Task<Cart?> FindByUserIdAsync(Guid userId, CancellationToken ct)
    {
        var filter = Builders<Cart>.Filter.Eq(cart => cart.CustomerId, userId);
        
        Cart? foundCart = await _cartCollection.Find(filter).FirstOrDefaultAsync(ct);

        return foundCart;
    }

    public Task<PaginatedList<Cart>> GetByFilterAsync(GetCartsQueryFilter queryFilter, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
    
    public async Task<bool> DeleteAsync(Guid userId, CancellationToken ct)
    {
        var filter = Builders<Cart>.Filter.Eq(cart => cart.CustomerId, userId);
        DeleteResult deleteResult = await _cartCollection.DeleteOneAsync(filter, ct);
        return deleteResult.DeletedCount == 1;
    }

    public async Task<Cart> UpsertAsync(Cart upsertingCart, CancellationToken ct)
    {
        if (upsertingCart.Id == Guid.Empty)
        {
            upsertingCart.Id = Guid.NewGuid();
        }

        var filter = Builders<Cart>.Filter.Eq(cart => cart.Id, upsertingCart.Id);
        var update = Builders<Cart>.Update
            .Set(cart => cart.CustomerId, upsertingCart.CustomerId)
            .Set(cart => cart.Products, upsertingCart.Products)
            .Set(cart => cart.Date, upsertingCart.Date);

        var options = new UpdateOptions { IsUpsert = true };

        await _cartCollection.UpdateOneAsync(filter, update, options, ct);

        return upsertingCart;
    }
}