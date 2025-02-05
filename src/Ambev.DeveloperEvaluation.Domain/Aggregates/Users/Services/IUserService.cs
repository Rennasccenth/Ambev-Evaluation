using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;

namespace Ambev.DeveloperEvaluation.Domain.Services.Abstractions;

public interface IUserService
{
    Task<Cart> GetUserCartAsync(Guid userId);
}