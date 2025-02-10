using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Exceptions;

public sealed class DuplicatedCartException : DomainException
{
    public DuplicatedCartException(string message) : base(message)
    {
    }

    public DuplicatedCartException(string message, Exception innerException) : base(message, innerException)
    {
    }
}