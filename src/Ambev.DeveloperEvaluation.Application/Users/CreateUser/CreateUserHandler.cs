using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, CommandResult<CreateUserResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly TimeProvider _timeProvider;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(
        IUserRepository userRepository,
        IMapper mapper,
        TimeProvider timeProvider,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _timeProvider = timeProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<CommandResult<CreateUserResult>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        User? existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingUser is not null) return ApplicationError.DuplicatedResourceError();

        var userBuildResult = User
            .GetBuilder(
                _passwordHasher, 
                _timeProvider, 
                new UserValidator())
            .WithEmail(command.Email)
            .WithUsername(command.Username)
            .WithPassword(command.Password)
            .WithAddress(new Address(
                command.City,
                command.Street,
                command.Number,
                command.ZipCode,
                command.Latitude,
                command.Longitude))
            .WithFirstname(command.Firstname)
            .WithLastname(command.Lastname)
            .WithPhone(command.Phone)
            .WithStatus(command.Status)
            .WithRole(command.Role)
            .Build();

        if (userBuildResult.IsT1)
        {
            return ApplicationError.ValidationError(userBuildResult.AsT1);
        }

        User storedUser = await _userRepository.CreateAsync(userBuildResult.AsT0, cancellationToken);
        return _mapper.Map<CreateUserResult>(storedUser);
    }
}
