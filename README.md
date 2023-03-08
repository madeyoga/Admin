# Admin
Automatic admin interface for aspnetcore. It reads metadata from entity type registered in options and DbContext.

```cs
using MyAdmin.Admin;

public class DummyModel
{
    [Key]
    public int Id { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = "test@gmail.com";

    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Note { get; set; } = "Test";

    [Required]
    [DataType(DataType.Date)]
    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    [DataType(DataType.DateTime)]
    public DateTime? CreatedDateTime { get; set; }

    [ForeignKey("ContentTypeId")]
    public ContentType? ContentType { get; set; }
}

public class MyDbContext : DbContext
{
    public DbSet<ContentType> ContentTypes { get; set; }
    public DbSet<DummyModel> DummyModels { get; set; }
}
```

```cs
// Program.cs

builder.Services.AddAdmin<MyDbContext>(options =>
{
    options.Register<ContentType>();
    options.Register<DummyModel>();
    options.SiteName = "aspnetcore";
});

...

app.MapGroup("/admin/")
   .MapAdminEndpoints<MyDbContext>();
```
