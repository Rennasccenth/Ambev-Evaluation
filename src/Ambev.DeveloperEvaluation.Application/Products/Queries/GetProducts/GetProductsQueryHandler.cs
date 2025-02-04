using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ApplicationResult<GetProductsQueryResult>>
{
    private readonly IProductRegistryRepository _productRegistryRepository;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IProductRegistryRepository productRegistryRepository, IMapper mapper)
    {
        _productRegistryRepository = productRegistryRepository;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<GetProductsQueryResult>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var getProductsQueryFilter = _mapper.Map<GetRegisteredProductsQueryFilter>(request);
        var products = await _productRegistryRepository.GetByFilterAsync(getProductsQueryFilter, cancellationToken);

        return _mapper.Map<GetProductsQueryResult>(products);
    }
}