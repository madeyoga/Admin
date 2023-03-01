using MyAdmin.Admin;
using MyAdmin.Admin.Widgets;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyAdmin.Fields;
public class DateTimeField : Field
{
    private DateTime _datetime;

    public DateTimeField(Widget widget, PropertyInfo property) : base(widget, property)
    {
    }

    public override object? GetValue()
    {
        return _datetime;
    }

    public override void Validate()
    {
        object? val = GetWidget.GetValue();
        if (val != null)
        {
            try
            {
                _datetime = DateTime.Parse(val.ToString()!);
            }
            catch (FormatException)
            {
                throw new ValidationException("Invalid format");
            }
        }
        else
        {
            if (!TypeHelper.IsNullable(Property.PropertyType))
            {
                throw new ValidationException($"Unable to assign value of null to a non nullable field: {Property.Name}");
            }
        }
    }
}
