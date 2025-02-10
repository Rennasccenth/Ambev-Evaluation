using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;
using Ambev.DeveloperEvaluation.Domain.Common;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.MongoDB.Repositories;

public sealed class CartRepository : ICartRepository
{
    private readonly IMongoCollection<Cart> _cartCollection;
    private readonly ILogger<CartRepository> _logger;

    public CartRepository(IMongoCollection<Cart> cartCollection, ILogger<CartRepository> logger)
    {
        _cartCollection = cartCollection;
        _logger = logger;
    }

    public async Task<Cart?> FindByUserIdAsync(Guid userId, CancellationToken ct)
    {
        var filter = Builders<Cart>.Filter.Eq(cart => cart.UserId, userId);
        
        Cart? foundCart = await _cartCollection.Find(filter).FirstOrDefaultAsync(ct);

        return foundCart;
    }

    public async Task<Cart?> FindByCartIdAsync(Guid cartId, CancellationToken ct)
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

    public async Task<bool> UpdateAsync(Cart updatingCart, CancellationToken ct)
    {
        var filter = Builders<Cart>.Filter.Eq(c => c.Id, updatingCart.Id);

        ReplaceOneResult? replaceResult = await _cartCollection
            .ReplaceOneAsync(filter, updatingCart, new ReplaceOptions { IsUpsert = false }, ct);

        return replaceResult.ModifiedCount > 0;
    }

    public async Task<Cart> UpsertAsync(Cart upsertingCart, CancellationToken ct)
    {
        if (upsertingCart.Id == Guid.Empty)
        {
            upsertingCart.Id = Guid.NewGuid();
        }

        var filter = Builders<Cart>.Filter.Eq(cart => cart.Id, upsertingCart.Id);
        var update = Builders<Cart>.Update
            .Set(cart => cart.UserId, upsertingCart.UserId)
            .Set(cart => cart.Products, upsertingCart.Products)
            .Set(cart => cart.Date, upsertingCart.Date);

        var options = new UpdateOptions { IsUpsert = true };

        await _cartCollection.UpdateOneAsync(filter, update, options, ct);

        return upsertingCart;
    }

    public async Task<Cart> CreateAsync(Cart newCart, CancellationToken ct = default)
    {
        var insertOptions = new InsertOneOptions
        {
            BypassDocumentValidation = true
        };

        try
        {
            await _cartCollection.InsertOneAsync(newCart, insertOptions, ct);
        }
        catch (Exception e)
        {
            _logger.LogError("Exception occurred when inserting a Cart: {ExceptionName} with message {ExceptionMessage}", e.GetType().Name, e.Message);
            throw new DuplicatedCartException($"CartId {newCart.Id} already exists.", e);
        }
        
        return newCart;
    }
}