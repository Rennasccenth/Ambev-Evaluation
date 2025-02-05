using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Exceptions;

public sealed class InvalidCartProductException : DomainException
{
    public InvalidCartProductException(string message) : base(message) { }

    public InvalidCartProductException(string message, Exception innerException) : base(message, innerException) { }
}