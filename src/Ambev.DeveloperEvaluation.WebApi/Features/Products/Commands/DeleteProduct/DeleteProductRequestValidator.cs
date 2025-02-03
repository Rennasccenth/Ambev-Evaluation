using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.DeleteProduct;

public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}