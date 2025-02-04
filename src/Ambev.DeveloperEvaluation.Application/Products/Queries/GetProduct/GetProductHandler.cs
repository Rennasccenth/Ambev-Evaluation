using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProduct;

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, ApplicationResult<GetProductQueryResult>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<GetProductQueryResult>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        Product? foundProduct = await _productRepository.FindByIdAsync(request.Id, cancellationToken);
        if (foundProduct is null) return ApplicationError.NotFoundError($"Product with ID {request.Id} wasn't found.");

        return _mapper.Map<GetProductQueryResult>(foundProduct);
    }
}