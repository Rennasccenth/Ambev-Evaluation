using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Ambev.DeveloperEvaluation.Common.ExceptionHandlers;

public static class ExceptionStatusCodeMapper
{
    public static int GetStatusCode(Exception exception) => exception switch
    {
        ValidationException => StatusCodes.Status400BadRequest,
        JsonException => StatusCodes.Status400BadRequest,
        ArgumentException => StatusCodes.Status400BadRequest,
        InvalidOperationException => StatusCodes.Status400BadRequest,
        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
        KeyNotFoundException => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
    };

    public static string GetTitle(Exception exception) => exception switch
    {
        ValidationException => "Validation Error",
        JsonException => "Invalid Request Format",
        ArgumentException => "Invalid Arguments",
        InvalidOperationException => "Invalid Operation",
        UnauthorizedAccessException => "Unauthorized",
        KeyNotFoundException => "Resource Not Found",
        _ => "Internal Server Error"
    };
}