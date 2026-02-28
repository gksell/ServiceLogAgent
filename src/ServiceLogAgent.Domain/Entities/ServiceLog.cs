namespace ServiceLogAgent.Domain.Entities;

public class ServiceLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? CorrelationId { get; set; }
    public string? TraceId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public long DurationMs { get; set; }
    public string? HttpMethod { get; set; }
    public string? Path { get; set; }
    public string? QueryString { get; set; }
    public string? RequestHeadersJson { get; set; }
    public string? RequestBody { get; set; }
    public int ResponseStatusCode { get; set; }
    public string? ResponseHeadersJson { get; set; }
    public string? ResponseBody { get; set; }
    public string? RemoteIp { get; set; }
    public string? UserId { get; set; }
    public string? ApplicationKey { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ExceptionStack { get; set; }
}
