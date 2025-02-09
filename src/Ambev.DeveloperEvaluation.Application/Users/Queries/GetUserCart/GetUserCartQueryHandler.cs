using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUserCart;

public sealed class GetUserCartQueryHandler : IRequestHandler<GetUserCartQuery, ApplicationResult<CartResult>>
{
    private readonly ICartsService _cartsService;
    private readonly IMapper _mapper;

    public GetUserCartQueryHandler(ICartsService cartsService, IMapper mapper)
    {
        _cartsService = cartsService;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<CartResult>> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
    {
        ApplicationResult<Cart> getUserCartResult = await _cartsService.GetCartByUserId(request.UserId, cancellationToken);

        return getUserCartResult.Match<ApplicationResult<CartResult>>(
            onSuccess: userCart => _mapper.Map<CartResult>(userCart),
            onFailure: error => error);
    }
}