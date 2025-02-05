using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Exceptions;

public sealed class SaleNotFoundException : DomainException
{
    public SaleNotFoundException(string message) : base(message)
    {
    }

    public SaleNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}