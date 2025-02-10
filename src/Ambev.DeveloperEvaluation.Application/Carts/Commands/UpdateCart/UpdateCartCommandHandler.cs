using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Abstractions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands.UpdateCart;

public sealed class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, ApplicationResult<CartResult>>
{
    private readonly ICartsService _cartsService;
    private readonly IUserContext _userContext;
    private readonly ILogger<UpdateCartCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateCartCommandHandler(
        ICartsService cartsService,
        IUserContext userContext,
        ILogger<UpdateCartCommandHandler> logger,
        IMapper mapper)
    {
        _cartsService = cartsService;
        _userContext = userContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<CartResult>> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating cart {CartId}", request.CartId);

        if (_userContext.IsAuthenticated)
        {
            return ApplicationError.BadRequestError("User is already authenticated, update the cart by calling the proper endpoint.");
        }

        var getCartResult = await _cartsService.GetCartById(request.CartId, cancellationToken);
        return await getCartResult.MatchAsync(
            onSuccess: async existentCart =>
            {
                foreach (ProductSummary productSummary in request.Products)
                {
                    existentCart.UpsertProduct(productSummary.ProductId, productSummary.Quantity);
                }
                var updateCartResult = await _cartsService.UpdateCartAsync(existentCart, cancellationToken);

                return updateCartResult.Match<ApplicationResult<CartResult>>(
                    onSuccess: updatedCart => _mapper.Map<CartResult>(updatedCart),
                    onFailure: err => err);
            },
            onFailure: err => err);
    }
}