using Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUserCart;
using Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUserCart;
using Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUser;
using Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUserCart;
using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUser;
using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUserCart;
using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUserCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.DeleteUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.DeleteUserCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUserCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUserCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUsers;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

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
    [EndpointSummary("Create a new user")]
    [EndpointDescription("Creates a new user in the system.")]
    [OpenApiOperation("Create a new user", "Creates a new user in the system.")]
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
            createUserResult => CreatedAtAction(nameof(GetUser), 
                new { UserId = createUserResult.Id } ,
                _mapper.Map<CreateUserResponse>(createUserResult)),
            error => HandleKnownError(error)
        );
    }
    
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Get a user by Id")]
    [EndpointDescription("Retrieves a user by their unique identifier.")]
    [OpenApiOperation("Get a user", "Retrieves a user by their unique identifier.")]
    public async Task<IActionResult> GetUser(
        [FromRoute] Guid userId,
        [FromServices] IValidator<GetUserRequest> getUserRequestValidator,
        CancellationToken cancellationToken)
    {
        GetUserRequest request = new () { Id = userId };
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

    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Delete a user by Id")]
    [EndpointDescription("Deletes a user by their unique identifier.")]
    [OpenApiOperation("Delete a user", "Deletes a user by their unique identifier.")]
    public async Task<IActionResult> DeleteUser(
        [FromRoute] Guid userId,
        [FromServices] IValidator<DeleteUserRequest> deleteUserRequestValidator,
        CancellationToken cancellationToken)
    {
        DeleteUserRequest request = new(userId);
        ValidationResult? validationResult = await deleteUserRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        var commandResult = await _mediator.Send(_mapper.Map<DeleteUserCommand>(request.Id), cancellationToken);

        return commandResult.Match(
            _ => NoContent(),
            error => HandleKnownError(error)
        );
    }

    [HttpPut("{userId:guid}")]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Update a user by Id")]
    [EndpointDescription("Updates a user by their unique identifier.")]
    [OpenApiOperation("Update a user", "Updates a user by their unique identifier.")]
    public async Task<IActionResult> UpdateUser(
        [FromRoute] Guid userId,
        [FromBody] UpdateUserRequestBody requestBody,
        [FromServices] IValidator<UpdateUserRequest> updateUserRequestValidator,
        CancellationToken cancellationToken)
    {
        UpdateUserRequest request = new(userId, requestBody);
        ValidationResult? validationResult = await updateUserRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        var commandResult = await _mediator.Send(_mapper.Map<UpdateUserCommand>(request), cancellationToken);

        return commandResult.Match(
            updatedUser => Ok(_mapper.Map<UpdateUserResponse>(updatedUser)),
            error => HandleKnownError(error)
        );
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetUsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [EndpointSummary("Get users")]
    [EndpointDescription("Retrieves a list of users based on the provided filters.")]
    [OpenApiOperation("Get users", "Retrieves a list of users based on the provided filters.")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] GetUsersRequest request,
        [FromServices] IValidator<GetUsersRequest> requestValidator,
        CancellationToken cancellationToken)
    {
        request.SetFilter(HttpContext.Request.Query);
        ValidationResult? validationResult = await requestValidator
            .ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        GetUsersQuery? query = _mapper.Map<GetUsersQuery>(request);
        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(
            users => Ok(_mapper.Map<GetUsersResponse>(users)),
            error => HandleKnownError(error)
        );
    }

    [Authorize]
    [HttpGet("{userId:guid}/cart")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Get user's cart - 🔐")]
    [EndpointDescription("Retrieves a user's cart by their unique identifier. [Requires a authenticated user]")]
    [OpenApiOperation("Get user's cart", "Retrieves a user's cart by their unique identifier. [Requires a authenticated user]")]
    public async Task<IActionResult> GetCartByUserId(
        [FromRoute] Guid userId,
        [FromServices] IValidator<GetUserCartRequest> requestValidator,
        CancellationToken ct)
    {
        GetUserCartRequest request = new (userId);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var getCartQuery = _mapper.Map<GetUserCartQuery>(request);
        var result = await _mediator.Send(getCartQuery, ct);
    
        return result.Match(
            onSuccess: successResult => Ok(_mapper.Map<CartResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    [Authorize]
    [HttpDelete("{userId:guid}/cart")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Delete user's cart- 🔐")]
    [EndpointDescription("Deletes a user's cart by their unique identifier. [Requires a authenticated user]")]
    [OpenApiOperation("Delete user's cart", "Deletes a user's cart by their unique identifier. [Requires a authenticated user]")]
    public async Task<IActionResult> DeleteUserCart(
        [FromRoute] Guid userId,
        [FromServices] IValidator<DeleteUserCartRequest> requestValidator,
        CancellationToken ct)
    {
        DeleteUserCartRequest request = new(userId);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<DeleteUserCartCommand>(request);
        var deleteCartCommandResult = await _mediator.Send(command, ct);
    
        return deleteCartCommandResult.Match(
            onSuccess: _ => NoContent(),
            onFailure: HandleKnownError);
    }

    [Authorize]
    [HttpPost("{userId:guid}/cart")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Create user's cart - 🔐")]
    [EndpointDescription("Creates a user's cart by their unique identifier. [Requires a authenticated user]")]
    [OpenApiOperation("Create user's cart", "Creates a user's cart by their unique identifier. [Requires a authenticated user]")]
    public async Task<IActionResult> CreateUserCart(
        [FromRoute] Guid userId,
        [FromBody] CreateUserCartRequestSummary requestSummary,
        [FromServices] IValidator<CreateUserCartRequest> requestValidator,
        CancellationToken ct)
    {
        CreateUserCartRequest request = new(userId, requestSummary.Date, requestSummary.Products);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<CreateUserCartCommand>(request);
        var commandResult = await _mediator.Send(command, ct);
    
        return commandResult.Match(
            onSuccess: successResult => Ok(_mapper.Map<CartResponse>(successResult)),
            onFailure: HandleKnownError);
    }
    
    [HttpPut("{userId:guid}/cart")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Update user's cart - 🔐")]
    [EndpointDescription("Updates a user's cart by their unique identifier. [Requires a authenticated user]")]
    [OpenApiOperation("Update user's cart", "Updates a user's cart by their unique identifier. [Requires a authenticated user]")]
    public async Task<IActionResult> UpdateUserCart(
        [FromRoute] Guid userId,
        [FromBody] UpdateCartProductsRequest productsRequest,
        [FromServices] IValidator<UpdateUserCartRequest> requestValidator,
        CancellationToken ct)
    {
        UpdateUserCartRequest request = new(userId, productsRequest.Products);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<UpdateUserCartCommand>(request);
        var commandResult = await _mediator.Send(command, ct);
    
        return commandResult.Match(
            onSuccess: successResult => Ok(_mapper.Map<CartResponse>(successResult)),
            onFailure: HandleKnownError);
    }
}