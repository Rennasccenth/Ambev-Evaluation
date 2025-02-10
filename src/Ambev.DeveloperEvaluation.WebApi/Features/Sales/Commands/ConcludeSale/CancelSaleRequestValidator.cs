using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Commands.ConcludeSale;

public sealed class ConcludeSaleRequestValidator : AbstractValidator<ConcludeSaleRequest>
{
    public ConcludeSaleRequestValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
    }
}