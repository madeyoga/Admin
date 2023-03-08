using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyAdmin.Fields;

public class FieldService<TContext> : IFieldService 
    where TContext : DbContext
{
    private readonly TContext context;
    private readonly FieldFactory fieldFactory;

    public FieldService(TContext context, FieldFactory fieldFactory)
    {
        this.context = context;
        this.fieldFactory = fieldFactory;
    }

    public IEntityType? FindEntityType(Type type)
    {
        return context.Model.FindEntityType(type);
    }

    public EntityEntry CreateEntityEntry(object instance)
    {
        return context.Entry(instance);
    }

    public Field CreateForeignKeyField(IProperty property)
    {
        throw new NotImplementedException();
    }
}
