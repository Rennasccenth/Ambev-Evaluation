using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Specifications;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

public sealed class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, ApplicationResult<AuthenticateUserResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticateUserHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ApplicationResult<AuthenticateUserResult>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
        {
            // Don't send back any message, we are avoiding leaking info about which field is incorrect.
            return ApplicationError.UnauthorizedAccessError();
        }

        ActiveUserSpecification activeUserSpec = new();
        if (!activeUserSpec.IsSatisfiedBy(user))
        {
            return ApplicationError.PermissionDeniedError($"The User {user.Email} is not active in the system.");
        }

        string token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticateUserResult { Token = token };
    }
}