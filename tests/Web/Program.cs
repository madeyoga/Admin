using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyAdmin.Admin;
using MyAdmin.Admin.Models;
using MyAdmin.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseInMemoryDatabase(databaseName: "Test");
    }
});

builder.Services.AddAdmin<MyDbContext>(options =>
{
    options.Register<ContentType, ModelAdmin>();
	options.Register<IdentityUser, ModelAdmin>();
	options.Register<DummyModel, ModelAdmin>();
	options.SiteName = "My New Admin";
});

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", () =>
{
    return Results.Extensions.Render("~/Templates/Index.cshtml");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseAdmin<MyDbContext>();

app.MapGroup("/admin/")
   .MapAdminEndpoints<MyDbContext>();

app.Run();
