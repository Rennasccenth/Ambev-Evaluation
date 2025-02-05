using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Services;

public interface IUserService
{
    Task<Cart> GetUserCartAsync(Guid userId);
}