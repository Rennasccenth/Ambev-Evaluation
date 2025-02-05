using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries;

public sealed class GetSaleQueryValidator : AbstractValidator<GetSaleQuery>
{
    public GetSaleQueryValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
    }
}