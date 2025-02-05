using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries;

public sealed class GetCartQuery : IRequest<ApplicationResult<CartResult>>
{
    public Guid UserId { get; set; }
}

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
        return _mapper.Map<CartResult>(await _cartsService.GetOrCreateUserCart(request.UserId, cancellationToken));
    }
}