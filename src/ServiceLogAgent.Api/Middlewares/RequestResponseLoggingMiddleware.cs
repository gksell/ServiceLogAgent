using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using ServiceLogAgent.Domain.Entities;
using ServiceLogAgent.Persistence.Context;

namespace ServiceLogAgent.Api.Middlewares;

public class RequestResponseLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestResponseLoggingMiddleware> logger)
{
    private const int MaxBodyBytes = 64 * 1024;
    private static readonly HashSet<string> SensitiveHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Authorization",
        "Cookie",
        "Set-Cookie"
    };

    public async Task InvokeAsync(HttpContext context, ServiceLogDbContext dbContext)
    {
        var startedAt = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew();

        var correlationId = context.Request.Headers.TryGetValue("X-Correlation-Id", out var incomingCorrelationId)
            ? incomingCorrelationId.ToString()
            : Guid.NewGuid().ToString("N");

        context.Response.Headers["X-Correlation-Id"] = correlationId;

        var requestBody = await ReadRequestBodyAsync(context.Request);
        var requestHeadersJson = SerializeHeaders(context.Request.Headers);

        var originalResponseBody = context.Response.Body;
        await using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        Exception? capturedException = null;

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            capturedException = ex;
            throw;
        }
        finally
        {
            stopwatch.Stop();

            responseBodyStream.Position = 0;
            var responseBody = await ReadStreamAsync(responseBodyStream);
            responseBodyStream.Position = 0;
            await responseBodyStream.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;

            var contextException = context.Items.TryGetValue(GlobalExceptionMiddleware.ExceptionItemKey, out var value)
                ? value as Exception
                : null;

            var effectiveException = contextException ?? capturedException;

            var serviceLog = new ServiceLog
            {
                CorrelationId = correlationId,
                TraceId = Activity.Current?.Id ?? context.TraceIdentifier,
                CreatedAtUtc = startedAt,
                DurationMs = stopwatch.ElapsedMilliseconds,
                HttpMethod = context.Request.Method,
                Path = context.Request.Path.ToString(),
                QueryString = context.Request.QueryString.Value,
                RequestHeadersJson = requestHeadersJson,
                RequestBody = Truncate(requestBody),
                ResponseStatusCode = context.Response.StatusCode,
                ResponseHeadersJson = SerializeHeaders(context.Response.Headers),
                ResponseBody = Truncate(responseBody),
                RemoteIp = context.Connection.RemoteIpAddress?.ToString(),
                UserId = ResolveUserId(context.User),
                ApplicationKey = context.Request.Headers["X-Application-Key"].ToString(),
                IsSuccess = context.Response.StatusCode is >= 200 and < 400,
                ErrorMessage = effectiveException?.Message,
                ExceptionStack = effectiveException?.ToString()
            };

            dbContext.ServiceLogs.Add(serviceLog);
            await dbContext.SaveChangesAsync();

            logger.LogInformation(
                "HTTP {Method} {Path} -> {StatusCode} in {ElapsedMs} ms (CorrelationId: {CorrelationId})",
                serviceLog.HttpMethod,
                serviceLog.Path,
                serviceLog.ResponseStatusCode,
                serviceLog.DurationMs,
                serviceLog.CorrelationId);
        }
    }

    private static async Task<string?> ReadRequestBodyAsync(HttpRequest request)
    {
        if (request.ContentLength is null or 0)
        {
            return null;
        }

        request.EnableBuffering();
        request.Body.Position = 0;
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    private static async Task<string?> ReadStreamAsync(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        var content = await reader.ReadToEndAsync();
        return string.IsNullOrWhiteSpace(content) ? null : content;
    }

    private static string SerializeHeaders(IHeaderDictionary headers)
    {
        var sanitized = headers.ToDictionary(
            kv => kv.Key,
            kv => SensitiveHeaders.Contains(kv.Key) ? "***MASKED***" : string.Join(";", kv.Value.ToArray()),
            StringComparer.OrdinalIgnoreCase);

        return JsonSerializer.Serialize(sanitized);
    }

    private static string? ResolveUserId(ClaimsPrincipal principal)
    {
        if (principal?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        return principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue("sub");
    }

    private static string? Truncate(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        if (Encoding.UTF8.GetByteCount(input) <= MaxBodyBytes)
        {
            return input;
        }

        var suffix = "(TRUNCATED)";
        var maxLength = input.Length;
        while (maxLength > 0 && Encoding.UTF8.GetByteCount(input[..maxLength] + suffix) > MaxBodyBytes)
        {
            maxLength--;
        }

        return input[..Math.Max(0, maxLength)] + suffix;
    }
}
