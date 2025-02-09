namespace Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUserCart;

public sealed class DeleteUserCartCommandResult
{
    public bool WasDeleted { get; }

    public DeleteUserCartCommandResult(bool wasDeleted)
    {
        WasDeleted = wasDeleted;
    }
}