using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ApplicationResult<GetProductsQueryResult>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<GetProductsQueryResult>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var getProductsQueryFilter = _mapper.Map<GetProductsQueryFilter>(request);
        var products = await _productRepository.GetByFilterAsync(getProductsQueryFilter, cancellationToken);

        return _mapper.Map<GetProductsQueryResult>(products);
    }
}