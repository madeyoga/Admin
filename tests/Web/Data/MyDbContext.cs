using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyAdmin.Admin.Models;

namespace MyAdmin.Data;

public class MyDbContext : IdentityDbContext
{
    public DbSet<ContentType> ContentTypes { get; set; }
    public DbSet<DummyModel> DummyModels { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {

    }
}
