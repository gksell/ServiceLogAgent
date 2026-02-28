namespace ServiceLogAgent.Application.Common;

public class GenericResponse<T>
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }
    public IReadOnlyList<string>? Errors { get; init; }

    public static GenericResponse<T> Ok(T? data, string message = "Success") =>
        new()
        {
            Success = true,
            Message = message,
            Data = data
        };

    public static GenericResponse<T> Fail(string message, params string[] errors) =>
        new()
        {
            Success = false,
            Message = message,
            Errors = errors
        };
}
