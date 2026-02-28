using ServiceLogAgent.Application.Abstractions;
using ServiceLogAgent.Application.Common;
using ServiceLogAgent.Application.Contracts;

namespace ServiceLogAgent.Application.Services;

public class PingService : IPingService
{
    public Task<GenericResponse<PingResult>> GetAsync()
    {
        return Task.FromResult(GenericResponse<PingResult>.Ok(new PingResult("ok")));
    }
}
