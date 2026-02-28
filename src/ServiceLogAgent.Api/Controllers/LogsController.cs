using Microsoft.AspNetCore.Mvc;
using ServiceLogAgent.Application.Abstractions;
using ServiceLogAgent.Application.Common;
using ServiceLogAgent.Application.Models;
using ServiceLogAgent.Domain.Entities;

namespace ServiceLogAgent.Api.Controllers;

[ApiController]
[Route("api/logs")]
public class LogsController(ILogService logService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GenericResponse<PagedResult<ServiceLog>>>> GetLogs(
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? toUtc,
        [FromQuery] int? statusCode,
        [FromQuery] string? contains,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var response = await logService.GetLogsAsync(new ServiceLogQuery(fromUtc, toUtc, statusCode, contains, page, pageSize));
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GenericResponse<ServiceLog>>> GetLogById(Guid id)
    {
        var response = await logService.GetByIdAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }
}
