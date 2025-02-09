using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUserCart;

public sealed class DeleteUserCartCommand : IRequest<ApplicationResult<DeleteUserCartCommandResult>>
{
    public DeleteUserCartCommand(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; init; }
}