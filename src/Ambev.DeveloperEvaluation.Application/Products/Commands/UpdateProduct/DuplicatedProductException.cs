namespace Ambev.DeveloperEvaluation.Application.Products.Commands.UpdateProduct;

public sealed class DuplicatedProductException : Exception
{
    public DuplicatedProductException(string msg) : base(msg) { }
}
