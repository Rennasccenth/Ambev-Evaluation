using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Products;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Products.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProduct;

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, ApplicationResult<GetProductQueryResult>>
{
    private readonly IProductRegistryRepository _productRegistryRepository;
    private readonly IMapper _mapper;

    public GetProductHandler(
        IProductRegistryRepository productRegistryRepository,
        IMapper mapper)
    {
        _productRegistryRepository = productRegistryRepository;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<GetProductQueryResult>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        Product? foundProduct = await _productRegistryRepository.FindByIdAsync(request.Id, cancellationToken);
        if (foundProduct is null) return ApplicationError.NotFoundError($"Product with ID {request.Id} wasn't found.");

        return _mapper.Map<GetProductQueryResult>(foundProduct);
    }
}