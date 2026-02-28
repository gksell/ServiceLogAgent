using ServiceLogAgent.Application.Abstractions;
using ServiceLogAgent.Application.Common;

namespace ServiceLogAgent.Application.Services;

public class EchoService : IEchoService
{
    public Task<GenericResponse<string?>> EchoAsync(string? body)
    {
        return Task.FromResult(GenericResponse<string?>.Ok(body));
    }
}
