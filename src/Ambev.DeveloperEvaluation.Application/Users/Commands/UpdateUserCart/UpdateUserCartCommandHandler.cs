using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUserCart;

public sealed class UpdateUserCartCommandHandler : IRequestHandler<UpdateUserCartCommand, ApplicationResult<CartResult>>
{
    private readonly ICartsService _cartsService;
    private readonly IMapper _mapper;

    public UpdateUserCartCommandHandler(
        ICartsService cartsService,
        IMapper mapper)
    {
        _cartsService = cartsService;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<CartResult>> Handle(UpdateUserCartCommand request, CancellationToken cancellationToken)
    {
        var userCartResult = await _cartsService.GetCartByUserId(request.UserId, cancellationToken);
        
        return await userCartResult.MatchAsync(
            onSuccess: async cart =>
            {
                foreach (ProductSummary requestProduct in request.Products)
                {
                    cart.UpsertProduct(requestProduct.ProductId, requestProduct.Quantity);
                }
                
                ApplicationResult<Cart> updatedCartResult = await _cartsService.UpdateCartAsync(cart, cancellationToken);
                return updatedCartResult.Match<ApplicationResult<CartResult>>(
                    onSuccess: updatedCart => _mapper.Map<CartResult>(updatedCart),
                    onFailure: error => error);
            },
            onFailure: error => error);
    }
}