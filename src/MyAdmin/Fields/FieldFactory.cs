namespace MyAdmin.Fields;

public class FieldFactory
{
    private readonly Dictionary<string, Type?> _fields;

    public FieldFactory()
    {
        _fields = new()
        {
            { typeof(string).Name, typeof(TextField) },
            { typeof(int).Name, typeof(IntegerField) },
            { typeof(float).Name, typeof(FloatField) },
            { typeof(double).Name, typeof(DoubleField) },
            { typeof(bool).Name, typeof(BooleanField) },
            { typeof(DateTime).Name, typeof(DateTimeField) },
        };
    }

    public Field? GetField(string name, params object?[]? args)
    {
        Type? fieldType = _fields.GetValueOrDefault(name, null);
        if (fieldType == null)
        {
            return null;
        }
        return Activator.CreateInstance(fieldType, args) as Field;
    }

    public Field? GetField(Type type, params object?[]? args)
    {
        return GetField(type.Name, args);
    }
}
