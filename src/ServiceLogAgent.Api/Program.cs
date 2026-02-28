using Serilog;
using ServiceLogAgent.Application;
using ServiceLogAgent.Api.Extensions;
using ServiceLogAgent.Api.Middlewares;
using ServiceLogAgent.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, services, config) =>
{
    config
        .ReadFrom.Configuration(ctx.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/servicelog-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14);
});

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

var app = builder.Build();

await app.MigrateDatabaseAsync();

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
