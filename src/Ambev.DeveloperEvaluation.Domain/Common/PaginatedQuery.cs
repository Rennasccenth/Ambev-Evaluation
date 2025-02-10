using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public abstract class PaginatedQuery
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }
    public Dictionary<string, string> FilterBy { get; set; } = [];
}

public sealed class PaginatedQueryValidator : AbstractValidator<PaginatedQuery> 
{
    public PaginatedQueryValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).LessThanOrEqualTo(100);
        RuleFor(x => x.OrderBy)
            .NotEmpty()
            .WithMessage("Invalid parameter format for order.")
            .When(x => x.OrderBy != null);
    }
}
