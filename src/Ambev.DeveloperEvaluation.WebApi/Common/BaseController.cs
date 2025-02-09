using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ambev.DeveloperEvaluation.Common.Errors;
using Microsoft.AspNetCore.WebUtilities;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected int GetCurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());

    protected string GetCurrentUserEmail() =>
        User.FindFirst(ClaimTypes.Email)?.Value ?? throw new NullReferenceException();

    /// <summary>
    /// Resolve the appropriate http response for every known error.
    /// </summary>
    protected IActionResult HandleKnownError(ApplicationError applicationError)
    {
        return applicationError switch
        {
            ValidationError validationError => ValidationProblem(new ValidationProblemDetails
            {
                Detail = validationError.Message,
                Status = StatusCodes.Status400BadRequest,
                Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest),
                Type = nameof(ValidationError),
                Errors = validationError.ErrorsDictionary
            }),
            BadRequestError badRequestError => Problem(
                detail: badRequestError.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest),
                type: nameof(BadRequestError)
            ),
            DuplicatedResourceError duplicatedResourceError => Problem(
                detail: duplicatedResourceError.Message,
                statusCode: StatusCodes.Status409Conflict,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status409Conflict),
                type: nameof(DuplicatedResourceError)
            ),
            InvalidArgumentError invalidArgumentError => Problem(
                detail: invalidArgumentError.Message,
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status422UnprocessableEntity),
                type: nameof(InvalidArgumentError)
            ),
            NotFoundError notFoundError => Problem(
                detail: notFoundError.Message,
                statusCode: StatusCodes.Status404NotFound,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status404NotFound),
                type: nameof(NotFoundError)
            ),
            PermissionDeniedError permissionDeniedError => Problem(
                detail: permissionDeniedError.Message,
                statusCode: StatusCodes.Status403Forbidden,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status403Forbidden),
                type: nameof(PermissionDeniedError)
            ),
            UnauthorizedAccessError unauthorizedAccessError => Problem(
                detail: unauthorizedAccessError.Message,
                statusCode: StatusCodes.Status401Unauthorized,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status401Unauthorized),
                type: nameof(UnauthorizedAccessError)
            ),
            UnprocessableError unprocessableError => Problem(
                detail: unprocessableError.Message,
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status422UnprocessableEntity),
                type: nameof(UnprocessableError)
            ),
            _ => Problem(
                detail: applicationError.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError),
                type: "Internal server error"
            )
        };
    }
}
