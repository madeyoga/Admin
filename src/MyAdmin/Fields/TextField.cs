using MyAdmin.Admin.Widgets;
using System.Reflection;

namespace MyAdmin.Fields;

public class TextField : Field
{
    private string value = "";

    public TextField(Widget widget, PropertyInfo property) : base(widget, property)
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
            value = val.ToString()!;
        }
    }
}
