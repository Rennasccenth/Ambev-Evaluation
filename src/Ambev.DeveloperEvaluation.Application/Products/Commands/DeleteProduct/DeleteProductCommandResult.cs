namespace Ambev.DeveloperEvaluation.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandResult
{
    public bool Deleted { get; }

    public DeleteProductCommandResult(bool deleted)
    {
        Deleted = deleted;
    }
}