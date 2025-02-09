using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUserCart;

public sealed class CreateUserCartCommandHandler : IRequestHandler<CreateUserCartCommand, ApplicationResult<CartResult>>
{
    private readonly ICartsService _cartsService;
    private readonly IMapper _mapper;

    public CreateUserCartCommandHandler(
        ICartsService cartsService,
        IMapper mapper)
    {
        _cartsService = cartsService;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<CartResult>> Handle(CreateUserCartCommand request, CancellationToken cancellationToken)
    {
        List<CartProduct> cartProducts = request.Products
            .Select(productSummary => new CartProduct(productSummary.ProductId, productSummary.Quantity))
            .ToList();
        
        ApplicationResult<Cart> createUserCartResult = await _cartsService.CreateUserCartAsync(request.UserId, cartProducts, cancellationToken);

        return createUserCartResult.Match(
            onSuccess: createdCart => _mapper.Map<CartResult>(createdCart),
            onFailure: error => ApplicationResult<CartResult>.Failure(error));
    }
}