using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace MyAdmin.Admin;

public static class RouteGroupBuilderExtensions
{
    public static void MapAdminEndpoints<TContext>(this RouteGroupBuilder group)
        where TContext : DbContext
    {
        group.MapGet("", AdminEndpoints.AdminIndex).WithName("MyAdmin_Index");
        group.MapGet("{modelName}/", AdminEndpoints.ModelIndex<TContext>).WithName("MyAdmin_ModelIndex");
        group.MapGet("{modelName}/add/", AdminEndpoints.ModelAdd_Get).WithName("MyAdmin_ModelAdd_Get");
        group.MapPost("{modelName}/add/", AdminEndpoints.ModelAdd_Post<TContext>).WithName("MyAdmin_ModelAdd_Post");
        group.MapGet("{modelName}/change/{objIdentifier}/", AdminEndpoints.ModelChange_Get<TContext>).WithName("MyAdmin_ModelChange_Get");
        group.MapPost("{modelName}/change/{objIdentifier}/", AdminEndpoints.ModelChange_Post<TContext>).WithName("MyAdmin_ModelChange_Post");
        group.WithGroupName("MyAdmin");
    }
}
