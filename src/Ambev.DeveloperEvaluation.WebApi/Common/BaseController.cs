using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ambev.DeveloperEvaluation.Common.Errors;
using Microsoft.AspNetCore.WebUtilities;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected int GetCurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());

    protected string GetCurrentUserEmail() =>
        User.FindFirst(ClaimTypes.Email)?.Value ?? throw new NullReferenceException();

    protected IActionResult Ok<T>(T data) =>
            base.Ok(new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult Created<T>(string routeName, object routeValues, T data) =>
        base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult OkPaginated<T>(PaginatedList<T> pagedList) =>
            Ok(new PaginatedResponse<T>
            {
                Data = pagedList,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                TotalCount = pagedList.TotalCount,
                Success = true
            });

    protected IActionResult HandleKnownError(ApplicationError applicationError)
    {
        return applicationError switch
        {
            BadRequestError => Problem(
                detail: applicationError.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest),
                type: nameof(BadRequestError)
            ),
            DuplicatedResourceError => Problem(
                detail: applicationError.Message,
                statusCode: StatusCodes.Status409Conflict,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status409Conflict),
                type: nameof(DuplicatedResourceError)
            ),
            InvalidArgumentError => Problem(
                detail: applicationError.Message,
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status422UnprocessableEntity),
                type: nameof(InvalidArgumentError)
            ),
            NotFoundError => Problem(
                detail: applicationError.Message,
                statusCode: StatusCodes.Status404NotFound,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status404NotFound),
                type: nameof(NotFoundError)
            ),
            PermissionDeniedError => Problem(
                detail: applicationError.Message,
                statusCode: StatusCodes.Status403Forbidden,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status403Forbidden),
                type: nameof(PermissionDeniedError)
            ),
            UnauthorizedAccessError => Problem(
                detail: applicationError.Message,
                statusCode: StatusCodes.Status401Unauthorized,
                title: ReasonPhrases.GetReasonPhrase(StatusCodes.Status401Unauthorized),
                type: nameof(UnauthorizedAccessError)
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
