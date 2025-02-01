using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

public sealed class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
}
