using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Abstractions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands.CreateCart;

public sealed class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, ApplicationResult<CartResult>>
{
    private readonly ICartsService _cartsService;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly ILogger<CreateCartCommandHandler> _logger;

    public CreateCartCommandHandler(
        ICartsService cartsService,
        IMapper mapper,
        IUserContext userContext,
        ILogger<CreateCartCommandHandler> logger)
    {
        _cartsService = cartsService;
        _mapper = mapper;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<ApplicationResult<CartResult>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating cart for user {UserId}", _userContext.UserId);
        if (_userContext.IsAuthenticated)
        {
            return ApplicationError.BadRequestError("User is already authenticated, create a cart in the proper endpoint.");
        }

        List<CartProduct> cartProducts = request.Products
            .Select(productSummary => new CartProduct(productSummary.ProductId, productSummary.Quantity))
            .ToList();

        ApplicationResult<Cart> createUserCartResult = request.Products.Count != 0
            ? await _cartsService.CreateGenericCart(cartProducts, cancellationToken)
            : await _cartsService.CreateGenericCart(cancellationToken);

        return createUserCartResult.Match(
            onSuccess: createdCart => _mapper.Map<CartResult>(createdCart),
            onFailure: error => ApplicationResult<CartResult>.Failure(error));
    }
}