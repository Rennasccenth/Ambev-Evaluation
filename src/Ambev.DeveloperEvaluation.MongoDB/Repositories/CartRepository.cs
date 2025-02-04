using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories.Carts;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.MongoDB.Repositories;

public sealed class CartRepository : ICartRepository
{
    private readonly IMongoCollection<Cart> _cartCollection;

    public CartRepository(IMongoDatabase mongoDatabase)
    {
        _cartCollection = mongoDatabase.GetCollection<Cart>(nameof(Cart).ToLowerInvariant());
    }

    public async Task<Cart?> FindByIdAsync(Guid cartId, CancellationToken ct)
    {
        var filter = Builders<Cart>.Filter.Eq(cart => cart.Id, cartId);
        
        Cart? foundCart = await _cartCollection.Find(filter).FirstOrDefaultAsync(ct);

        return foundCart;
    }

    public Task<PaginatedList<Cart>> GetByFilterAsync(GetCartsQueryFilter queryFilter, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
    
    public async Task<bool> DeleteAsync(Guid cartId, CancellationToken ct)
    {
        var filter = Builders<Cart>.Filter.Eq(cart => cart.Id, cartId);
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
        ReplaceOneResult replacedResult = await _cartCollection.ReplaceOneAsync(filter, upsertingCart, new ReplaceOptions
        {
            IsUpsert = true
        }, ct);

        if (replacedResult.UpsertedId != null) 
        {
            upsertingCart.Id = replacedResult.UpsertedId.AsGuid; 
        }

        return upsertingCart;
    }
}