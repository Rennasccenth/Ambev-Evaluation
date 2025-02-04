namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public sealed record Rating
{
    public decimal Rate { get; init; }
    public int Count { get; init; }

    internal Rating() { }

    public Rating(decimal rate, int count)
    {
        Rate = rate;
        Count = count;
    }
}