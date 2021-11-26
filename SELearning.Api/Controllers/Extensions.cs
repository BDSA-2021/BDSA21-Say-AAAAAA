using Microsoft.AspNetCore.Mvc;

namespace SELearning.API.Controllers;

public static class Extensions
{
    public static IActionResult ToActionResult(this OperationResult result)
        => result switch
        {
            OperationResult.Updated => new NoContentResult(),
            OperationResult.Deleted => new NoContentResult(),
            OperationResult.NotFound => new NotFoundResult(),
            _ => throw new NotSupportedException($"{result} not supported")
        };

    public static ActionResult<T> ToActionResult<T>(this Option<T> option) where T : class
        => option.IsSome ? option.Value : new NotFoundResult();
}
