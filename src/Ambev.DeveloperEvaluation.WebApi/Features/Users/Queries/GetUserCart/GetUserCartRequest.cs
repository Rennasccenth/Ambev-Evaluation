namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUserCart;

public sealed class GetUserCartRequest
{
    public Guid UserId { get; }

    public GetUserCartRequest(Guid userId)
    {
        UserId = userId;
    }
}