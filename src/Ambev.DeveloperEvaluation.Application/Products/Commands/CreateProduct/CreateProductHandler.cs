using Ambev.DeveloperEvaluation.Application.Products.Exceptions;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApplicationResult<CreateProductCommandResult>>
{
    private readonly IProductRegistryRepository _productRegistryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(IProductRegistryRepository productRegistryRepository, IMapper mapper, ILogger<CreateProductCommandHandler> logger)
    {
        _productRegistryRepository = productRegistryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApplicationResult<CreateProductCommandResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating product with Title: {Title}", request.Title);
        Product creatingProduct = new (request.Title, request.Price, request.Description, request.Category, request.Image, request.Rating);
        Product storedProduct;
        try
        {
             storedProduct = await _productRegistryRepository.CreateAsync(creatingProduct, cancellationToken);
        }
        catch (DuplicatedProductException e)
        {
            return ApplicationError.DuplicatedResourceError(e.Message);
        }

        _logger.LogInformation("Created product {Title} with Id: {ProductId}", request.Title, storedProduct.Id.ToString());
        return _mapper.Map<CreateProductCommandResult>(storedProduct);
    }
}