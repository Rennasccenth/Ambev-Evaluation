using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Specifications;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public sealed class CreateSaleCommand : IRequest<ApplicationResult<SaleResult>>
{
    public Guid UserId { get; set; }
    public Guid CartId { get; set; }
    public string Branch { get; set; } = null!;
}

public sealed class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, ApplicationResult<SaleResult>>
{
    private readonly IMapper _mapper;
    private readonly ISalesService _salesService;
    private readonly ICartRepository _cartRepository;
    private readonly IProductPriceResolver _priceResolver;
    private readonly ILogger<CreateSaleCommandHandler> _logger;

    public CreateSaleCommandHandler(
        IMapper mapper,
        ISalesService salesService,
        ICartRepository cartRepository,
        IProductPriceResolver priceResolver,
        ILogger<CreateSaleCommandHandler> logger)
    {
        _mapper = mapper;
        _salesService = salesService;
        _cartRepository = cartRepository;
        _priceResolver = priceResolver;
        _logger = logger;
    }

    public async Task<ApplicationResult<SaleResult>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating sale for user {UserId}", request.UserId);

        Cart? userCart = await _cartRepository.FindByUserIdAsync(request.UserId, cancellationToken);
        if (userCart is null)
        {
            return ApplicationError.NotFoundError("Cart not found for user.");
        }

        ItemQuantityLessThanSpecification itemRestrictionSpec = new();
        // Injecting this spec (it could be multiple also) allow us to be flexible regarding some promotions for instance.
        Sale createdSale = await _salesService.CreateSaleAsync(
            cart: userCart, 
            branch: request.Branch, 
            productPriceResolver: _priceResolver,
            specification: itemRestrictionSpec, 
            ct: cancellationToken);
        
        _logger.LogInformation("Sale created with id {SaleId}", createdSale.Id);
        return _mapper.Map<SaleResult>(createdSale);
    }
}