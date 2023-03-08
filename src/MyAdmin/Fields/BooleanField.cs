using Microsoft.EntityFrameworkCore.Metadata;
using MyAdmin.Admin.Widgets;
using System.Reflection;

namespace MyAdmin.Fields;
public class BooleanField : Field
{
    private bool _value;

    public BooleanField(Widget widget, PropertyInfo property) : base(widget, property)
    {
    }

    public BooleanField(Widget widget, IProperty property) : base(widget, property)
    {
    }

    public override object? GetValue()
    {
        return _value;
    }

    public override void Validate()
    {
        string? value = GetWidget.GetAttribute("value");
        
        if (string.IsNullOrEmpty(value))
        {
            _value = false;
        }

        if (string.Equals(value, "on", StringComparison.OrdinalIgnoreCase))
        {
            _value = true;
        }
        else
        {
            _value = false;
        }
    }
}
