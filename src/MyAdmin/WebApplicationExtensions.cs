using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyAdmin.Admin.Services;

namespace MyAdmin.Admin;

public static class WebApplicationExtensions
{
    public static WebApplication MapAdminEndpoints<TContext>(this WebApplication app)
        where TContext : DbContext
    {
        app.MapGet("", AdminEndpoints.AdminIndex);
        app.MapGet("{modelName}/", AdminEndpoints.ModelIndex<TContext>);
        app.MapGet("{modelName}/add/", AdminEndpoints.ModelAdd_Get);
        return app;
    }

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
