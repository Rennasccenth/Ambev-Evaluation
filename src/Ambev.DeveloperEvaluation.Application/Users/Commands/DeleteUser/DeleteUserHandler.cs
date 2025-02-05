using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Repositories.User;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUser;

public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand, ApplicationResult<DeleteUserCommandResult>>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationResult<DeleteUserCommandResult>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var success = await _userRepository.DeleteAsync(request.Id, cancellationToken);
        if (!success)
            return ApplicationError.NotFoundError($"User with ID {request.Id} not found.");

        return new DeleteUserCommandResult { Success = true };
    }
}
