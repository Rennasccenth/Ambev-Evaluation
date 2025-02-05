using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Exceptions;

public sealed class CartValidationException : DomainException
{
    public CartValidationException(string message) : base(message) { }

    public CartValidationException(string message, Exception innerException) : base(message, innerException) { }
}