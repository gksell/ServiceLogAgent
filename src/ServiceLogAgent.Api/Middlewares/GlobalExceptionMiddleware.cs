using Microsoft.AspNetCore.Mvc;

namespace ServiceLogAgent.Api.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public const string ExceptionItemKey = "ServiceLog.Exception";

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            context.Items[ExceptionItemKey] = ex;
            logger.LogError(ex, "Unhandled exception while processing request.");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = "See logs for details.",
                Instance = context.Request.Path
            };

            problem.Extensions["correlationId"] = context.Response.Headers["X-Correlation-Id"].ToString();

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
