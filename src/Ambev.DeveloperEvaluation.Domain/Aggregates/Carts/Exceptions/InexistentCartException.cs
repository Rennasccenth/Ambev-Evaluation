using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Exceptions;

public class InexistentCartException : DomainException
{
    public InexistentCartException(string message) : base(message)
    {
    }

    public InexistentCartException(string message, Exception innerException) : base(message, innerException)
    {
    }
}