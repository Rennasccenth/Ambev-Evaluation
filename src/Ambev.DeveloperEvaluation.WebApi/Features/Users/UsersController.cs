using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request,
        [FromServices] IValidator<CreateUserRequest> requestValidator,
        CancellationToken cancellationToken)
    {
        ValidationResult? validationResult = await requestValidator
            .ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        var command = _mapper.Map<CreateUserCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return response.Match(
            createUserResult => CreatedAtAction(nameof(GetUser), new { createUserResult.Id } ,createUserResult),
            error => HandleKnownError(error)
        );
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(
        [FromRoute] Guid id,
        [FromServices] IValidator<GetUserRequest> getUserRequestValidator,
        CancellationToken cancellationToken)
    {
        GetUserRequest request = new () { Id = id };
        ValidationResult? validationResult = await getUserRequestValidator
            .ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        GetUserCommand? command = _mapper.Map<GetUserCommand>(request.Id);
        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(
            foundUser => Ok(_mapper.Map<GetUserResponse>(foundUser)),
            error => HandleKnownError(error)
        );
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(
        [FromRoute] Guid id,
        [FromServices] IValidator<DeleteUserRequest> deleteUserRequestValidator,
        CancellationToken cancellationToken)
    {
        DeleteUserRequest request = new(id);
        ValidationResult? validationResult = await deleteUserRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        var commandResult = await _mediator.Send(_mapper.Map<DeleteUserCommand>(request.Id), cancellationToken);

        return commandResult.Match(
            _ => NoContent(),
            error => HandleKnownError(error)
        );
    }
}
