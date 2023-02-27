using MyAdmin.Admin.Widgets;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyAdmin.Fields;
public class DoubleField : Field
{
    private double value;

    public DoubleField(Widget widget, PropertyInfo property) : base(widget, property)
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
                value = double.Parse(val.ToString()!);
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
