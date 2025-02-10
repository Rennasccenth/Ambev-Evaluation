namespace Ambev.DeveloperEvaluation.Application.Carts.Commands.DeleteCart;

public sealed class DeleteCartCommandResult
{
    public bool WasDeleted { get; }

    public DeleteCartCommandResult(bool wasDeleted)
    {
        WasDeleted = wasDeleted;
    }
}