using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Category)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Image)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.Rating)
                .NotNull();
        }
    }
}