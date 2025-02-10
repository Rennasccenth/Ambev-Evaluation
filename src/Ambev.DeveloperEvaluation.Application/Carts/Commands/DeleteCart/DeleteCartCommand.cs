using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands.DeleteCart;

public sealed class DeleteCartCommand : IRequest<ApplicationResult<DeleteCartCommandResult>>
{
    public DeleteCartCommand(Guid cartId)
    {
        CartId = cartId;
    }

    public Guid CartId { get; }
}