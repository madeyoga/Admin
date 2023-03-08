using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyAdmin.Admin;
using Web.Data;
using Web.Models;

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
    options.Register<ContentType>();
    options.Register<IdentityUser>();
    options.Register<DummyModel>();
    options.Register<Product>();
	options.SiteName = "aspnetcore";
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
