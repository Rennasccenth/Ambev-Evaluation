using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUser;

/// <summary>
/// Validator for GetUserCommand
/// </summary>
public sealed class GetUserValidator : AbstractValidator<GetUserCommand>
{
    /// <summary>
    /// Initializes validation rules for GetUserCommand
    /// </summary>
    public GetUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}
