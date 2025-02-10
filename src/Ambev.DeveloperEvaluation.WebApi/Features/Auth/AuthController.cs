using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.Commands.AuthenticateUser;
using FluentValidation;
using FluentValidation.Results;
using NSwag.Annotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [EndpointSummary("Authenticate a User - [Public Access]")]
    [EndpointDescription("Authenticate a User, if their credential matches.")]
    [OpenApiOperation("Authenticate a User", "Authenticate a User, if their credential matches.")]
    public async Task<IActionResult> AuthenticateUser(
        [FromBody] AuthenticateUserRequest request,
        [FromServices] IValidator<AuthenticateUserRequest> requestValidator,
        CancellationToken cancellationToken)
    {
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return HandleKnownError(validationResult.Errors);

        var command = _mapper.Map<AuthenticateUserCommand>(request);
        ApplicationResult<AuthenticateUserResult> applicationResult = await _mediator.Send(command, cancellationToken);

        return applicationResult.Match<IActionResult>(
            success => Ok(_mapper.Map<AuthenticateUserResponse>(success)),
            error => HandleKnownError(error)
        );
    }
}
