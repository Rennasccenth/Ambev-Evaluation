using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUserCart;

public sealed class DeleteUserCartCommandValidator : AbstractValidator<DeleteUserCartCommand>
{
    public DeleteUserCartCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}