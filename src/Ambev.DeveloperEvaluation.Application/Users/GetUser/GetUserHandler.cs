using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

public sealed class GetUserHandler : IRequestHandler<GetUserCommand, CommandResult<GetUserResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserHandler(
        IUserRepository userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<CommandResult<GetUserResult>> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        User? foundUser = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundUser is null) 
            return ApplicationError.NotFoundError($"User with ID {request.Id} wasn't found.");
        return _mapper.Map<GetUserResult>(foundUser);
    }
}
