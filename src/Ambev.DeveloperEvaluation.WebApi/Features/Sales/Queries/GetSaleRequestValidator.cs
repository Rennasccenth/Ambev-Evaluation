using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Queries;

public sealed class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
{
    public GetSaleRequestValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
    }
}