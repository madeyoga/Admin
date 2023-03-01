using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyAdmin.Admin;

public static class TypeHelper
{
    public static Type FindKeyType(DbContext context, Type modelType)
    {
        IEntityType? entityType = context.Model.FindEntityType(modelType);
        if (entityType == null)
        {
            throw new InvalidOperationException($"Couldn't find entity {modelType.Name} from dbcontext");
        }
        IKey? pk = entityType.FindPrimaryKey();
        if (pk == null)
        {
            throw new InvalidOperationException($"No primary key found for entity type {modelType.Name}");
        }
        IProperty? pkProperty = pk.Properties.FirstOrDefault()!;
        if (pkProperty == null)
        {
            throw new InvalidOperationException($"No primary key found for entity type '{modelType.Name}'");
        }

        return pkProperty.ClrType;
    }

    public static string? FindPrimaryKeyName(Type type)
    {
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            if (property.IsDefined(typeof(KeyAttribute), false))
            {
                return property.Name;
            }
        }
        return null;
    }

    public static bool IsNullable(Type type)
    {
        return Nullable.GetUnderlyingType(type) != null;
    }
}
