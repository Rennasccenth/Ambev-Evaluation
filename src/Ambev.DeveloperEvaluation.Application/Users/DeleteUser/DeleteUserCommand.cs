using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

public sealed class DeleteUserCommand : IRequest<CommandResult<DeleteUserResponse>>
{
    public Guid Id { get; }

    public DeleteUserCommand(Guid id)
    {
        Id = id;
    }
}
