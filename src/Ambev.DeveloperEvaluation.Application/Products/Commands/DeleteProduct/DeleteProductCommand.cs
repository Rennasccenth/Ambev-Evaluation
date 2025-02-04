using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommand : IRequest<ApplicationResult<DeleteProductCommandResult>>
{
    public Guid Id { get; }

    public DeleteProductCommand(Guid id)
    {
        Id = id;
    }
}