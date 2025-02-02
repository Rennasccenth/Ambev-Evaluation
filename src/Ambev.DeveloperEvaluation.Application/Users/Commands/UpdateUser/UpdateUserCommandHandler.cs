using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.User;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler 
    : IRequestHandler<UpdateUserCommand, ApplicationResult<UpdateUserCommandResult>>
{
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly TimeProvider _timeProvider;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserCommandHandler(
        ILogger<UpdateUserCommandHandler> logger,
        IUserRepository userRepository,
        IMapper mapper,
        TimeProvider timeProvider,
        IPasswordHasher passwordHasher)
    {
        _logger = logger;
        _userRepository = userRepository;
        _mapper = mapper;
        _timeProvider = timeProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApplicationResult<UpdateUserCommandResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating user with ID {Id}", request.Id);
        
        User? updatingUser = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (updatingUser is null) return ApplicationError.NotFoundError($"User ID {request.Id} not found.");

        updatingUser.UpdateUsername(request.Username, _timeProvider);
        updatingUser.ChangePassword(request.Password, _passwordHasher, _timeProvider);
        updatingUser.UpdateAddress(request.Address, _timeProvider);
        updatingUser.UpdateEmail(request.Email, _timeProvider);
        updatingUser.UpdatePhone(request.Phone, _timeProvider);
        updatingUser.UpdateRole(request.Role, _timeProvider);
        updatingUser.UpdateStatus(request.Status, _timeProvider);

        User updatedUser = await _userRepository.UpdateAsync(updatingUser, cancellationToken);
        var updatedUserCommandResult = _mapper.Map<UpdateUserCommandResult>(updatedUser);
        
        _logger.LogInformation("User with ID {Id} updated successfully", request.Id);
        return updatedUserCommandResult;
    }
}