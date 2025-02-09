namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.DeleteUserCart;

public sealed class DeleteUserCartRequest
{
    public Guid UserId { get; init; }
    public DeleteUserCartRequest(Guid userId)
    {
        UserId = userId;
    }
}