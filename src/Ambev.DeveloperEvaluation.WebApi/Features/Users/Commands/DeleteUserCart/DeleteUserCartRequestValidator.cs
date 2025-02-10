using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.DeleteUserCart;

public sealed class DeleteUserCartRequestValidator : AbstractValidator<DeleteUserCartRequest>
{
    public DeleteUserCartRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}