using Microsoft.Extensions.DependencyInjection;
using ServiceLogAgent.Application.Abstractions;
using ServiceLogAgent.Application.Services;

namespace ServiceLogAgent.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPingService, PingService>();
        services.AddScoped<IEchoService, EchoService>();
        services.AddScoped<IErrorService, ErrorService>();
        services.AddScoped<ILogService, LogService>();
        return services;
    }
}
