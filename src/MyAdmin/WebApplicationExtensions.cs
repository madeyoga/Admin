using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyAdmin.Admin;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Populate content types
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="app"></param>
    /// <returns>Current WebApplication instance</returns>
    public static WebApplication UseAdmin<TContext>(this WebApplication app)
        where TContext : DbContext
    {
        // Render a dotnet local tool for content types management.
        using var scope = app.Services.CreateScope();
        var ctInitializer = scope.ServiceProvider.GetRequiredService<ContentTypeInitializer<TContext>>();
        ctInitializer.Initialize();

        return app;
    }
}
