using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

public sealed class GetUserCommand : IRequest<CommandResult<GetUserResult>>
{
    public Guid Id { get; }

    public GetUserCommand(Guid id)
    {
        Id = id;
    }
}
