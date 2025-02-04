using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}
