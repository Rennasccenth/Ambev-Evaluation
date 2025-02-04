using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetCategories;

public sealed class GetCategoriesQuery : IRequest<ApplicationResult<GetCategoriesQueryResponse>>;

public sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ApplicationResult<GetCategoriesQueryResponse>>
{
    private readonly IProductRegistryRepository _productRegistryRepository;

    public GetCategoriesQueryHandler(IProductRegistryRepository productRegistryRepository)
    {
        _productRegistryRepository = productRegistryRepository;
    }

    public async Task<ApplicationResult<GetCategoriesQueryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _productRegistryRepository.GetCategoriesAsync(cancellationToken);

        return new GetCategoriesQueryResponse(categories);
    }
}