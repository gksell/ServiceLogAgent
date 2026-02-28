using ServiceLogAgent.Application.Abstractions;
using ServiceLogAgent.Application.Common;
using ServiceLogAgent.Application.Models;
using ServiceLogAgent.Domain.Entities;

namespace ServiceLogAgent.Application.Services;

public class LogService(IGenericRepository<ServiceLog> repository) : ILogService
{
    public async Task<GenericResponse<PagedResult<ServiceLog>>> GetLogsAsync(ServiceLogQuery query)
    {
        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 1, 200);

        IQueryable<ServiceLog> ApplyFilters(IQueryable<ServiceLog> dataQuery)
        {
            if (query.FromUtc.HasValue)
            {
                dataQuery = dataQuery.Where(x => x.CreatedAtUtc >= query.FromUtc.Value);
            }

            if (query.ToUtc.HasValue)
            {
                dataQuery = dataQuery.Where(x => x.CreatedAtUtc <= query.ToUtc.Value);
            }

            if (query.StatusCode.HasValue)
            {
                dataQuery = dataQuery.Where(x => x.ResponseStatusCode == query.StatusCode.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Contains))
            {
                dataQuery = dataQuery.Where(x =>
                    (x.Path != null && x.Path.Contains(query.Contains)) ||
                    (x.RequestBody != null && x.RequestBody.Contains(query.Contains)) ||
                    (x.ResponseBody != null && x.ResponseBody.Contains(query.Contains)) ||
                    (x.ErrorMessage != null && x.ErrorMessage.Contains(query.Contains)));
            }

            return dataQuery;
        }

        var totalCount = await repository.CountAsync(ApplyFilters);
        var items = await repository.ListAsync(q => ApplyFilters(q)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize));

        var paged = new PagedResult<ServiceLog>(items, page, pageSize, totalCount);
        return GenericResponse<PagedResult<ServiceLog>>.Ok(paged);
    }

    public async Task<GenericResponse<ServiceLog>> GetByIdAsync(Guid id)
    {
        var item = await repository.FirstOrDefaultAsync(x => x.Id == id);
        return item is null
            ? GenericResponse<ServiceLog>.Fail("Log not found", $"No log exists for id '{id}'.")
            : GenericResponse<ServiceLog>.Ok(item);
    }
}
