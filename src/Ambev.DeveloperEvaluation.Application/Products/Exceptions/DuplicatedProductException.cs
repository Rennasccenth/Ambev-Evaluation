namespace Ambev.DeveloperEvaluation.Application.Products.Exceptions;

public sealed class DuplicatedProductException : Exception
{
    public  DuplicatedProductException() : base("Product already exists") { }
    private DuplicatedProductException(string msg) : base(msg) { }
}
