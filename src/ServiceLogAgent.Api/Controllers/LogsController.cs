using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLogAgent.Application.Models;
using ServiceLogAgent.Domain.Entities;
using ServiceLogAgent.Persistence.Context;

namespace ServiceLogAgent.Api.Controllers;

[ApiController]
[Route("api/logs")]
public class LogsController(ServiceLogDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ServiceLog>>> GetLogs(
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? toUtc,
        [FromQuery] int? statusCode,
        [FromQuery] string? contains,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 200);

        IQueryable<ServiceLog> query = dbContext.ServiceLogs.AsNoTracking();

        if (fromUtc.HasValue)
        {
            query = query.Where(x => x.CreatedAtUtc >= fromUtc.Value);
        }

        if (toUtc.HasValue)
        {
            query = query.Where(x => x.CreatedAtUtc <= toUtc.Value);
        }

        if (statusCode.HasValue)
        {
            query = query.Where(x => x.ResponseStatusCode == statusCode.Value);
        }

        if (!string.IsNullOrWhiteSpace(contains))
        {
            query = query.Where(x =>
                (x.Path != null && x.Path.Contains(contains)) ||
                (x.RequestBody != null && x.RequestBody.Contains(contains)) ||
                (x.ResponseBody != null && x.ResponseBody.Contains(contains)) ||
                (x.ErrorMessage != null && x.ErrorMessage.Contains(contains)));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PagedResult<ServiceLog>(items, page, pageSize, totalCount));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceLog>> GetLogById(Guid id)
    {
        var item = await dbContext.ServiceLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : Ok(item);
    }
}
