using Microsoft.EntityFrameworkCore;
using Serilog;
using ServiceLogAgent.Api.Extensions;
using ServiceLogAgent.Api.Middlewares;
using ServiceLogAgent.Persistence.Context;

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

builder.Services.AddDbContext<ServiceLogDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

await app.MigrateDatabaseAsync();

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
