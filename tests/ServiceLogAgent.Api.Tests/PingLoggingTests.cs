using System.Net;
using Microsoft.Extensions.DependencyInjection;
using ServiceLogAgent.Api.Tests.Infrastructure;
using ServiceLogAgent.Persistence.Context;

namespace ServiceLogAgent.Api.Tests;

public class PingLoggingTests : IClassFixture<ServiceLogWebApplicationFactory>
{
    private readonly ServiceLogWebApplicationFactory _factory;

    public PingLoggingTests(ServiceLogWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Ping_CreatesLogRow()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/ping");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ServiceLogDbContext>();
        var count = db.ServiceLogs.Count(x => x.Path == "/api/ping");

        Assert.True(count >= 1);
    }
}
