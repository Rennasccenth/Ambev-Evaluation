using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

/// <summary>
/// Validator for GetUserRequest
/// </summary>
internal sealed class GetUserRequestValidator : AbstractValidator<GetUserRequest>
{
    /// <summary>
    /// Initializes validation rules for GetUserRequest
    /// </summary>
    internal GetUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}
