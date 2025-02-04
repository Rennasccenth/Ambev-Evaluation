using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUser;

public sealed class DeleteUserCommand : IRequest<ApplicationResult<DeleteUserCommandResult>>
{
    public Guid Id { get; }

    public DeleteUserCommand(Guid id)
    {
        Id = id;
    }
}
