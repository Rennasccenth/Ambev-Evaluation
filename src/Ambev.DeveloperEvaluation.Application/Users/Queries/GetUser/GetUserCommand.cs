using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUser;

public sealed class GetUserCommand : IRequest<ApplicationResult<GetUserResult>>
{
    public Guid Id { get; }

    public GetUserCommand(Guid id)
    {
        Id = id;
    }
}
