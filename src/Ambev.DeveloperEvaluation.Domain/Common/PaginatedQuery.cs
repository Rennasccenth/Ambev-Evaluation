using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public abstract class PaginatedQuery
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
}

public sealed class PaginatedQueryValidator : AbstractValidator<PaginatedQuery> 
{
    public PaginatedQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).LessThanOrEqualTo(100);
        RuleFor(x => x.OrderBy)
            .NotEmpty()
            .WithMessage("Invalid parameter format for order.")
            .When(x => x.OrderBy != null);
    }
}
