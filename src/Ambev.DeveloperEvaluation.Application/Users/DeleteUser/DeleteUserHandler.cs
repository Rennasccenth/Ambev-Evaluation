using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand, CommandResult<DeleteUserResponse>>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CommandResult<DeleteUserResponse>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var success = await _userRepository.DeleteAsync(request.Id, cancellationToken);
        if (!success)
            return ApplicationError.NotFoundError($"User with ID {request.Id} not found.");

        return new DeleteUserResponse { Success = true };
    }
}
