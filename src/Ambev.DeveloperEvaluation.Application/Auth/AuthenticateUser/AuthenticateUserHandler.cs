using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using OneOf;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

public sealed class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, OneOf<AuthenticateUserResult, ApplicationError>>
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

    public async Task<OneOf<AuthenticateUserResult, ApplicationError>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            
        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
        {
            return ApplicationError.ValidationError(new ValidationErrorDetail(error: nameof(request.Password), detail: "Password is incorrect."));
        }

        ActiveUserSpecification activeUserSpec = new ();
        if (!activeUserSpec.IsSatisfiedBy(user))
        {
            return ApplicationError.UnauthorizedAccessError("User is not active.");
        }

        string token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticateUserResult
        {
            Token = token,
            Email = user.Email,
            Name = user.Username,
            Role = user.Role.ToString()
        };
    }
}