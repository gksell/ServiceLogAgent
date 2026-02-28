namespace ServiceLogAgent.Application.Models;

public sealed record ServiceLogQuery(
    DateTime? FromUtc,
    DateTime? ToUtc,
    int? StatusCode,
    string? Contains,
    int Page,
    int PageSize);
