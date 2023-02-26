using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
}
