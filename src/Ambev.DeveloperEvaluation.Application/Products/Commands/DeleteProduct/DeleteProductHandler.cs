using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Products.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand, ApplicationResult<DeleteProductCommandResult>>
{
    private readonly IProductRegistryRepository _productRegistryRepository;
    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(IProductRegistryRepository productRegistryRepository, ILogger<DeleteProductHandler> logger)
    {
        _productRegistryRepository = productRegistryRepository;
        _logger = logger;
    }

    public async Task<ApplicationResult<DeleteProductCommandResult>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting product with ID {Id}", command.Id);
        var wasDeleted = await _productRegistryRepository.DeleteAsync(command.Id, cancellationToken);

        if (!wasDeleted) return ApplicationError.NotFoundError($"Product ID {command.Id} wasn't found.");

        _logger.LogInformation("Product with ID {Id} was deleted", command.Id);
        return new DeleteProductCommandResult(wasDeleted);
    }
}