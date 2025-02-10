using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Commands.CancelSale;

public sealed class CancelSaleRequestValidator : AbstractValidator<CancelSaleRequest>
{
    public CancelSaleRequestValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
    }
}