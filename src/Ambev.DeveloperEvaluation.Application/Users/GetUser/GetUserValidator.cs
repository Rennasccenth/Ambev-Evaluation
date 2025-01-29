using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Validator for GetUserCommand
/// </summary>
internal sealed class GetUserValidator : AbstractValidator<GetUserCommand>
{
    /// <summary>
    /// Initializes validation rules for GetUserCommand
    /// </summary>
    internal GetUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}
