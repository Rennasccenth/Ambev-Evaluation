using Ambev.DeveloperEvaluation.Application.Products.Exceptions;
using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ApplicationResult<UpdateProductCommandResult>>
{
    private readonly IProductRegistryRepository _productRegistryRepository;
    private readonly IMapper _mapper;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(
        IProductRegistryRepository productRegistryRepository,
        IMapper mapper,
        TimeProvider timeProvider,
        ILogger<UpdateProductHandler> logger)
    {
        _productRegistryRepository = productRegistryRepository;
        _mapper = mapper;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task<ApplicationResult<UpdateProductCommandResult>> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating product {Id}", request.Id);

        Product? updatingProduct = await _productRegistryRepository.FindByIdAsync(request.Id, cancellationToken);
        if (updatingProduct is null) return ApplicationError.NotFoundError($"Product ID {request.Id} wasn't found.");

        updatingProduct = updatingProduct
            .ChangeTitle(request.Title)
            .ChangePrice(request.Price, _timeProvider)
            .ChangeDescription(request.Description)
            .ChangeCategory(request.Category)
            .ChangeImage(request.Image)
            .ChangeRating(request.Rating);

        Product updatedProduct;
        try
        {
            updatedProduct = await _productRegistryRepository.UpdateAsync(updatingProduct, cancellationToken);
        }
        catch (DuplicatedProductException e)
        {
            return  ApplicationError.DuplicatedResourceError(e.Message);
        }

        _logger.LogInformation("Product {Id} updated", request.Id);
        return _mapper.Map<UpdateProductCommandResult>(updatedProduct);
    }
}