using ServiceLogAgent.Application.Common;

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
            context.Response.ContentType = "application/json";

            var response = GenericResponse<object>.Fail(
                "An unexpected error occurred.",
                ex.Message);
            var envelope = new
            {
                response.Success,
                response.Message,
                response.Errors,
                CorrelationId = context.Response.Headers["X-Correlation-Id"].ToString()
            };

            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}
