using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand, ApplicationResult<DeleteProductCommandResult>>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ApplicationResult<DeleteProductCommandResult>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var wasDeleted = await _productRepository.DeleteAsync(command.Id, cancellationToken);

        if (!wasDeleted) return ApplicationError.NotFoundError($"Product ID {command.Id} wasn't found.");

        return new DeleteProductCommandResult(wasDeleted);
    }
}