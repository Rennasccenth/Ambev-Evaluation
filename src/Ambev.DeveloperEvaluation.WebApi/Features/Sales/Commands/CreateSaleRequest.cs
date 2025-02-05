using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Queries;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;
using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Commands;

public sealed class CreateSaleRequest
{
    public Guid CartId { get; set; }
    public Guid UserId { get; set; }
    public string Branch { get; set; }
}

public sealed class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Branch).NotEmpty().MinimumLength(5);
    }
}

public sealed class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        // In
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        
    }
}