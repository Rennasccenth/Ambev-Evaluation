using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApplicationResult<CreateProductCommandResult>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<CreateProductCommandResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product creatingProduct = new (request.Title, request.Price, request.Description, request.Category, request.Image, request.Rating);

        Product storedProduct = await _productRepository.CreateAsync(creatingProduct, cancellationToken);

        return _mapper.Map<CreateProductCommandResult>(storedProduct);
    }
}