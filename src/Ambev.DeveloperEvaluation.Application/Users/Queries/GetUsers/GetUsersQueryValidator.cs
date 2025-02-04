using Ambev.DeveloperEvaluation.Domain.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;

public sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator(
        IValidator<PaginatedQuery> baseValidator)
    {
        Include(baseValidator);

        When(x => x.FilterBy.Values.Count > 0, () =>
        {
            RuleFor(x => x.FilterBy.Values)
                .Must(x => x.All(v => !string.IsNullOrEmpty(v)))
                .WithMessage("All filter values must be provided");
        });
    }
}