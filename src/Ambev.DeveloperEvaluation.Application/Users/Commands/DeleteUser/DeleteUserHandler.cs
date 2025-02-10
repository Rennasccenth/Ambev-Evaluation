using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Services;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUser;

public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand, ApplicationResult<UserResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public DeleteUserHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<UserResult>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User? foundUser = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundUser is null)
            return ApplicationError.NotFoundError($"User with ID {request.Id} not found.");
        
        var success = await _userRepository.DeleteAsync(request.Id, cancellationToken);
        if (!success)
            return ApplicationError.UnprocessableError("Unable to delete user.");


        return _mapper.Map<UserResult>(foundUser);
    }
}
