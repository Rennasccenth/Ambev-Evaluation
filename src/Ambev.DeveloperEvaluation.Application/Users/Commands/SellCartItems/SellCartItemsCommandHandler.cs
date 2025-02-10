using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Abstractions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Specifications;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.SellCartItems;

public sealed class SellCartItemsCommandHandler : IRequestHandler<SellCartItemsCommand, ApplicationResult<SaleResult>>
{
    private readonly IMapper _mapper;
    private readonly ISalesService _salesService;
    private readonly ICartsService _cartsService;
    private readonly IUserContext _userContext;
    private readonly IProductPriceResolver _priceResolver;
    private readonly ILogger<SellCartItemsCommandHandler> _logger;
    private static readonly ItemQuantityLessThanSpecification ItemRestrictionSpec = new();
    public SellCartItemsCommandHandler(
        IMapper mapper,
        ISalesService salesService,
        IProductPriceResolver priceResolver,
        ILogger<SellCartItemsCommandHandler> logger,
        ICartsService cartsService,
        IUserContext userContext)
    {
        _mapper = mapper;
        _salesService = salesService;
        _priceResolver = priceResolver;
        _logger = logger;
        _cartsService = cartsService;
        _userContext = userContext;
    }

    public async Task<ApplicationResult<SaleResult>> Handle(SellCartItemsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating sale for user {UserId}", request.UserId);

        if (_userContext.UserId != request.UserId || !_userContext.IsAuthenticated)
        {
            return ApplicationError.PermissionDeniedError("User must be logged and be the owner of the selling cart.");
        }

        var userCartResult = await _cartsService.GetCartByUserId(request.UserId, cancellationToken);
        if (userCartResult.Error is not null) return userCartResult.Error;

        if(!ItemRestrictionSpec.IsSatisfiedBy(userCartResult.Data!))
        {
            return ApplicationError.UnprocessableError("User's cart exceeded the item quantity threshold.");
        }

        ApplicationResult<Sale> createSaleResult = await _salesService.CreateSaleAsync(
            cart: userCartResult.Data!, 
            branch: request.BranchName, 
            productPriceResolver: _priceResolver,
            specification: ItemRestrictionSpec, 
            ct: cancellationToken);

        return createSaleResult.Match<ApplicationResult<SaleResult>>(
            onSuccess: createdSale =>
            {
                _logger.LogInformation("Sale created with id {SaleId}", createdSale.Id);
                return _mapper.Map<SaleResult>(createdSale);
            },
            onFailure: error => error);
    }
}