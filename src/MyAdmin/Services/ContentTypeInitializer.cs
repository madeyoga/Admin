using Microsoft.EntityFrameworkCore;
using MyAdmin.Admin.Models;
using System.Reflection;

namespace MyAdmin.Admin.Services;

public class ContentTypeInitializer<TContext> where TContext : DbContext
{
    private readonly TContext _context;

    public ContentTypeInitializer(TContext context)
    {
        _context = context;
    }

    public void Initialize()
    {
        foreach (PropertyInfo propertyInfo in _context.GetType().GetProperties())
        {
            Type propType = propertyInfo.PropertyType;
            DbSet<ContentType> ContentTypes = _context.Set<ContentType>();
            if (propType.IsGenericType && propType.Name == typeof(DbSet<>).Name)
            {
                Type modelType = propType.GetGenericArguments().First();
                if (ContentTypes.Any(ct => ct.FullName == modelType.FullName && ct.Model == modelType.Name))
                {
                    continue;
                }
                ContentTypes.Add(new ContentType()
                {
                    Model = modelType.Name,
                    FullName = modelType.FullName!,
                });
            }
        }
        _context.SaveChanges();
    }
}
