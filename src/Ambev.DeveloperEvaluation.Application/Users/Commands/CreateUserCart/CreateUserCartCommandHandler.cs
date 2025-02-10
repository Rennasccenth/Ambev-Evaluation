using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Abstractions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUserCart;

public sealed class CreateUserCartCommandHandler : IRequestHandler<CreateUserCartCommand, ApplicationResult<CartResult>>
{
    private readonly ICartsService _cartsService;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public CreateUserCartCommandHandler(
        ICartsService cartsService,
        IUserContext userContext,
        IMapper mapper)
    {
        _cartsService = cartsService;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<CartResult>> Handle(CreateUserCartCommand request, CancellationToken cancellationToken)
    {
        if (!_userContext.IsAuthenticated)
        {
            return ApplicationError.UnauthorizedAccessError("Cannot create a cart for a unauthenticated user.");
        }

        List<CartProduct> cartProducts = request.Products
            .Select(productSummary => new CartProduct(productSummary.ProductId, productSummary.Quantity))
            .ToList();

        ApplicationResult<Cart> createUserCartResult = request.Products.Count != 0
            ? await _cartsService.CreateUserCartAsync(request.UserId, cartProducts, cancellationToken)
            : await _cartsService.CreateUserCartAsync(request.UserId, cancellationToken);

        return createUserCartResult.Match(
            onSuccess: createdCart => _mapper.Map<CartResult>(createdCart),
            onFailure: error => ApplicationResult<CartResult>.Failure(error));
    }
}