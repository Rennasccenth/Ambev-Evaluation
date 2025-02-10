using Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUserCart;
using Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUserCart;
using Ambev.DeveloperEvaluation.Application.Users.Commands.SellCartItems;
using Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUser;
using Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUserCart;
using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUser;
using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUserCart;
using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Abstractions;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUserCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.DeleteUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.DeleteUserCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.SellCartItems;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUserCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;
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
    [Authorize(Roles="Admin")]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [EndpointSummary("Create a new user - 🔐 [Only for Admin Users]")]
    [EndpointDescription("Creates a new user in the system.")]
    [OpenApiOperation("Create a new user - 🔐 [Only for Admin Users]", "Creates a new user in the system.")]
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
    [Authorize(Roles="Admin")]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Get a user by Id - 🔐 [Only for Admin Users]")]
    [EndpointDescription("Retrieves a user by their unique identifier.")]
    [OpenApiOperation("Get a user - 🔐 [Only for Admin Users]", "Retrieves a user by their unique identifier.")]
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
    [Authorize(Roles="Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Delete a user by Id - 🔐 [Only for Admin Users]")]
    [EndpointDescription("Deletes a user by their unique identifier.")]
    [OpenApiOperation("Delete a user - 🔐 [Only for Admin Users]", "Deletes a user by their unique identifier.")]
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
            deletedUser => Ok(_mapper.Map<UserResponse>(deletedUser)),
            error => HandleKnownError(error)
        );
    }

    [HttpPut("{userId:guid}")]
    [Authorize(Roles="Admin")]
    [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Update a user by Id - 🔐 [Only for Admin Users]")]
    [EndpointDescription("Updates a user by their unique identifier.")]
    [OpenApiOperation("Update a user - 🔐 [Only for Admin Users]", "Updates a user by their unique identifier.")]
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
    [Authorize(Roles="Admin")]
    [ProducesResponseType(typeof(GetUsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [EndpointSummary("Get users - 🔐 [Only for Admin Users]")]
    [EndpointDescription("Retrieves a list of users based on the provided filters.")]
    [OpenApiOperation("Get users - 🔐 [Only for Admin Users]", "Retrieves a list of users based on the provided filters.")]
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

    [HttpGet("me/cart")]
    [Authorize(Roles="Customer")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Get user's cart - 🔐 [Only for Customers Users]")]
    [EndpointDescription("Retrieves a user's cart by the user unique identifier. [Requires a authenticated user]")]
    [OpenApiOperation("Get user's cart - 🔐 [Only for Customers Users]", "Retrieves a user's cart by their unique identifier. [Requires a authenticated user]")]
    public async Task<IActionResult> GetUserCart(
        [FromServices] IValidator<GetUserCartRequest> requestValidator,
        [FromServices] IUserContext userContext,
        CancellationToken ct)
    {
        if (userContext.UserId is null) return HandleKnownError(ApplicationError.UnauthorizedAccessError());

        GetUserCartRequest request = new (userContext.UserId.Value);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var getCartQuery = _mapper.Map<GetUserCartQuery>(request);
        var result = await _mediator.Send(getCartQuery, ct);
    
        return result.Match(
            onSuccess: successResult => Ok(_mapper.Map<CartResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    [HttpDelete("me/cart")]
    [Authorize(Roles="Customer")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Delete user's cart - 🔐 [Only for Customers Users]")]
    [EndpointDescription("Deletes a user's cart by the user unique identifier. [Requires a authenticated user]")]
    [OpenApiOperation("Delete user's cart - 🔐 [Only for Customers Users]", "Deletes a user's cart by their unique identifier. [Requires a authenticated user]")]
    public async Task<IActionResult> DeleteUserCart(
        [FromServices] IValidator<DeleteUserCartRequest> requestValidator,
        [FromServices] IUserContext userContext,
        CancellationToken ct)
    {
        if (userContext.UserId is null) return HandleKnownError(ApplicationError.UnauthorizedAccessError());

        DeleteUserCartRequest request = new(userContext.UserId.Value);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<DeleteUserCartCommand>(request);
        var deleteCartCommandResult = await _mediator.Send(command, ct);
    
        return deleteCartCommandResult.Match(
            onSuccess: _ => NoContent(),
            onFailure: HandleKnownError);
    }

    [HttpPost("me/cart")]
    [Authorize(Roles="Customer")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [EndpointSummary("Create user's cart - 🔐 [Only for Customers Users]")]
    [EndpointDescription("Creates a user's cart by the user unique identifier. [Requires a authenticated user]")]
    [OpenApiOperation("Create user's cart - 🔐 [Only for Customers Users]", "Creates a user's cart by their unique identifier. [Requires a authenticated user]")]
    public async Task<IActionResult> CreateUserCart(
        [FromBody] CreateUserCartRequestSummary requestSummary,
        [FromServices] IValidator<CreateUserCartRequest> requestValidator,
        [FromServices] IUserContext userContext,
        CancellationToken ct)
    {
        if (userContext.UserId is null) return HandleKnownError(ApplicationError.UnauthorizedAccessError());

        CreateUserCartRequest request = new(userContext.UserId.Value, requestSummary.Date, requestSummary.Products);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<CreateUserCartCommand>(request);
        var commandResult = await _mediator.Send(command, ct);
        
        return commandResult.Match(
            onSuccess: createUserCartResult => CreatedAtAction(nameof(GetUserCart), 
                null,
                _mapper.Map<CartResponse>(createUserCartResult)),
            onFailure: error => HandleKnownError(error)
        );
    }
    
    [HttpPut("me/cart")]
    [Authorize(Roles="Customer")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Update user's cart - 🔐 [Only for Customers Users]")]
    [EndpointDescription("Updates a user's cart by user unique identifier. [Requires a authenticated user]")]
    [OpenApiOperation("Update user's cart - 🔐 [Only for Customers Users]", "Updates a user's cart by their unique identifier. [Requires a authenticated user]")]
    public async Task<IActionResult> UpdateUserCart(
        [FromBody] UpdateCartProductsRequest productsRequest,
        [FromServices] IValidator<UpdateUserCartRequest> requestValidator,
        [FromServices] IUserContext userContext,
        CancellationToken ct)
    {
        if (userContext.UserId is null) return HandleKnownError(ApplicationError.UnauthorizedAccessError());

        UpdateUserCartRequest request = new(userContext.UserId.Value, productsRequest.Products);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<UpdateUserCartCommand>(request);
        var commandResult = await _mediator.Send(command, ct);
    
        return commandResult.Match(
            onSuccess: successResult => Ok(_mapper.Map<CartResponse>(successResult)),
            onFailure: HandleKnownError);
    }
    
    [HttpPost("me/cart/checkout")]
    [Authorize(Roles="Customer")]
    [ProducesResponseType(typeof(SaleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [EndpointSummary("Purchase cart items - 🔐 [Only for Customers Users]")]
    [EndpointDescription("Creates a new sale from items in user's cart. [Requires a authenticated user]")]
    [OpenApiOperation("Purchase cart items - 🔐 [Only for Customers Users]", "Creates a new sale from items in user's cart. [Requires a authenticated user]")]
    public async Task<IActionResult> SellCartItems(
        [FromBody] SellCartItemsRequestBody requestBody,
        [FromServices] IValidator<SellCartItemsRequest> requestValidator,
        [FromServices] IUserContext userContext,
        CancellationToken ct)
    {
        if (userContext.UserId is null) return HandleKnownError(ApplicationError.UnauthorizedAccessError());

        SellCartItemsRequest request = new(userContext.UserId.Value, requestBody.BranchName);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<SellCartItemsCommand>(request);
        var commandResult = await _mediator.Send(command, ct);
    
        return commandResult.Match(
            onSuccess: successResult => Ok(_mapper.Map<SaleResponse>(successResult)),
            onFailure: HandleKnownError);
    }
}