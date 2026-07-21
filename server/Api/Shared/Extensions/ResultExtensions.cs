using Domain.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Shared.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkResult();

        return result.Error!.Type switch
        {
            ErrorType.Validation => new BadRequestObjectResult(result.Error.Message),
            ErrorType.NotFound => new NotFoundObjectResult(result.Error.Message),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(result.Error.Message),
            ErrorType.Forbidden => new ObjectResult(result.Error.Message) { StatusCode = 403 },
            ErrorType.Conflict => new ConflictObjectResult(result.Error.Message),
            ErrorType.Internal => new ObjectResult(result.Error.Message) { StatusCode = 500 },
            _ => new ObjectResult(result.Error.Message) { StatusCode = 500 },
        };
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);
        return ((Result)result).ToActionResult();
    }

    public static IActionResult ToCreatedAtActionResult<T>(
        this Result<T> result,
        string actionName,
        object? routeValues = null)
    {
        if (result.IsSuccess)
            return new CreatedAtActionResult(actionName, null, routeValues, result.Value);
        return ((Result)result).ToActionResult();
    }
}
