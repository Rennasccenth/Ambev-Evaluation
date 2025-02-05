using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands;

public sealed class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, ApplicationResult<CartResult>>
{
    private readonly ICartsService _cartService;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCartCommandHandler> _logger;

    public CreateCartCommandHandler(ICartsService cartService, IMapper mapper, ILogger<CreateCartCommandHandler> logger)
    {
        _cartService = cartService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApplicationResult<CartResult>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating cart for user {UserId}", request.UserId);
        
        var productQuantityRelation = request.Products.Select(p => new { p.ProductId, p.Quantity})
            .ToDictionary(p => p.ProductId, p => (uint)p.Quantity);
        Cart updatedCart = await _cartService.UpsertCartProductsAsync(request.UserId, productQuantityRelation, cancellationToken);

        _logger.LogInformation("Cart created for user {UserId}", request.UserId);
        return _mapper.Map<CartResult>(updatedCart);
    }
}