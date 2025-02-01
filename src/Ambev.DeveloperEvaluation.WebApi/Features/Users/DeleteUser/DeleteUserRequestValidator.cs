using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;

public sealed class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}
