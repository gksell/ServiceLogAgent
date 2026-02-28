using Microsoft.EntityFrameworkCore;
using ServiceLogAgent.Persistence.Context;

namespace ServiceLogAgent.Api.Extensions;

public static class DevelopmentDatabaseExtensions
{
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ServiceLogDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
