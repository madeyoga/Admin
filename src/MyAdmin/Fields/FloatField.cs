using Microsoft.EntityFrameworkCore.Metadata;
using MyAdmin.Admin.Widgets;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyAdmin.Fields;

public class FloatField : Field
{
    private float? value;

    public FloatField(Widget widget, PropertyInfo property) : base(widget, property)
    {
    }

    public FloatField(Widget widget, IProperty property) : base(widget, property)
    {
    }

    public override object? GetValue()
    {
        return value;
    }

    public override void Validate()
    {
        object? val = GetWidget.GetValue();
        if (val != null)
        {
            try
            {
                value = float.Parse(val.ToString()!);
            }
            catch (FormatException)
            {
                throw new ValidationException("Invalid format");
            }
            catch (OverflowException)
            {
                throw new ValidationException("Overflow");
            }
        }
    }
}
