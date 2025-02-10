using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.ConcludeSale;

public sealed class ConcludeSaleCommandValidator : AbstractValidator<ConcludeSaleCommand>
{
    public ConcludeSaleCommandValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
    }
}