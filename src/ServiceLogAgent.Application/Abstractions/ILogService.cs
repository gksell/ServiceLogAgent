using ServiceLogAgent.Application.Common;
using ServiceLogAgent.Application.Models;
using ServiceLogAgent.Domain.Entities;

namespace ServiceLogAgent.Application.Abstractions;

public interface ILogService
{
    Task<GenericResponse<PagedResult<ServiceLog>>> GetLogsAsync(ServiceLogQuery query);
    Task<GenericResponse<ServiceLog>> GetByIdAsync(Guid id);
}
