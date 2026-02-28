using ServiceLogAgent.Application.Common;

namespace ServiceLogAgent.Application.Abstractions;

public interface IEchoService
{
    Task<GenericResponse<string?>> EchoAsync(string? body);
}
