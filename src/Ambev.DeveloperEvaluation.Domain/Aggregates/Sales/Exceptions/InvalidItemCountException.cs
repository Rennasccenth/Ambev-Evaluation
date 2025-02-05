using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Exceptions;

public sealed class InvalidItemCountException : DomainException
{
    public InvalidItemCountException(string message) : base(message)
    {
    }

    public InvalidItemCountException(string message, Exception innerException) : base(message, innerException)
    {
    }
}