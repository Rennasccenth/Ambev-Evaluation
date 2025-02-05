using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.User;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUser;

public sealed class GetUserHandler : IRequestHandler<GetUserCommand, ApplicationResult<GetUserResult>>
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

    public async Task<ApplicationResult<GetUserResult>> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        User? foundUser = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundUser is null) 
            return ApplicationError.NotFoundError($"User with ID {request.Id} wasn't found.");
        return _mapper.Map<GetUserResult>(foundUser);
    }
}
