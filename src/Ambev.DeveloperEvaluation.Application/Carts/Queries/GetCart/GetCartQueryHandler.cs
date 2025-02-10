using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCart;

public sealed class GetCartQueryHandler : IRequestHandler<GetCartQuery, ApplicationResult<CartResult>>
{
    private readonly ICartsService _cartsService;
    private readonly IMapper _mapper;

    public GetCartQueryHandler(ICartsService cartsService, IMapper mapper)
    {
        _cartsService = cartsService;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<CartResult>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        ApplicationResult<Cart> getCartResult = await _cartsService.GetCartById(request.CartId, cancellationToken);

        return getCartResult.Match<ApplicationResult<CartResult>>(
            onSuccess: cart => _mapper.Map<CartResult>(cart),
            onFailure: error => error
        );
    }
}