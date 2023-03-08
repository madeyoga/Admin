using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyAdmin.Admin;
using Web.Models;

namespace Web.Data;

public class MyDbContext : IdentityDbContext
{
    public DbSet<ContentType> ContentTypes { get; set; }
    public DbSet<DummyModel> DummyModels { get; set; }
    public DbSet<Product> Products { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {

    }
}
