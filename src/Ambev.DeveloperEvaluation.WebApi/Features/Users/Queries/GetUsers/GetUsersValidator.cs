using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUsers;

public sealed class GetUsersValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersValidator()
    {
        RuleFor(req => req.Status)
            .IsEnumName(typeof(UserStatus), caseSensitive: false).When(req => !string.IsNullOrEmpty(req.Status));
        
        RuleFor(req => req.Role)
            .IsEnumName(typeof(UserRole), caseSensitive: false).When(req => !string.IsNullOrEmpty(req.Role));
    }
}