using ServiceLogAgent.Application.Common;
using ServiceLogAgent.Application.Contracts;

namespace ServiceLogAgent.Application.Abstractions;

public interface IPingService
{
    Task<GenericResponse<PingResult>> GetAsync();
}
