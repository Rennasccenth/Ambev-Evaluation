using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Exceptions;

public sealed class InvalidDiscountException : DomainException
{
    public InvalidDiscountException(string message) : base(message)
    {
    }

    public InvalidDiscountException(string message, Exception innerException) : base(message, innerException)
    {
    }
}