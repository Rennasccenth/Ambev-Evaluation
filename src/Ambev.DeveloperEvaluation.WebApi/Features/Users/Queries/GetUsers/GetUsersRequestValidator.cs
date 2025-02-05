using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUsers;

public sealed class GetUsersRequestValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersRequestValidator()
    {
        RuleFor(req => req.Status)
            .NotNull()
            .IsEnumName(typeof(UserStatus), caseSensitive: false).When(req => !string.IsNullOrEmpty(req.Status));

        RuleFor(req => req.Role)
            .NotNull()
            .IsEnumName(typeof(UserRole), caseSensitive: false).When(req => !string.IsNullOrEmpty(req.Role));
    }
}