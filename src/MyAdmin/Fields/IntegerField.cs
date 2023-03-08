using Microsoft.EntityFrameworkCore.Metadata;
using MyAdmin.Admin;
using MyAdmin.Admin.Widgets;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyAdmin.Fields;

public class IntegerField : Field
{
    private int? value;

    public IntegerField(Widget widget, PropertyInfo property) : base(widget, property)
    {
    }

    public IntegerField(Widget widget, IProperty property) : base(widget, property)
    {
    }

    public override void Validate()
    {
        object? val = GetWidget.GetValue();
        if (val != null)
        {
            try
            {
                value = int.Parse(val.ToString()!);
            }
            catch (FormatException)
            {
                throw new ValidationException();
            }
            catch (OverflowException)
            {
                throw new ValidationException();
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

    public override object? GetValue()
    {
        return value;
    }
}
