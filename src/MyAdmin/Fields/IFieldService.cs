using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyAdmin.Fields;

public interface IFieldService
{
    IEntityType? FindEntityType(Type type);
    EntityEntry CreateEntityEntry(object instance);
    Field CreateForeignKeyField(IProperty property);
}