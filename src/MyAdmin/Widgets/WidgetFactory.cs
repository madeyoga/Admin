using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyAdmin.Admin.Widgets;

public class WidgetFactory
{
    /// <summary>
    /// Map widget types by Type name
    /// </summary>
    private Dictionary<string, Type?> _widgets;

    /// <summary>
    /// Map widget types by DataType enum
    /// </summary>
    private Dictionary<DataType, Type?> _widgets2;

    public IReadOnlyDictionary<string, Type?> WidgetDictionary
    {
        get
        {
            return _widgets.AsReadOnly();
        }
    }

    public WidgetFactory()
    {
        Type dtWidgetType = typeof(DateTimeWidget);
        Type numberWidgetType = typeof(InputNumberWidget);
        _widgets2 = new()
        {
            { DataType.Password, typeof(PasswordWidget) },
            { DataType.EmailAddress, typeof(EmailWidget) },
            { DataType.MultilineText, typeof(TextareaWidget) },
            { DataType.Date, typeof(DateWidget) },
            { DataType.DateTime, dtWidgetType },
        };
        _widgets = new()
        {
            { typeof(string).Name, typeof(InputTextWidget) },
            { typeof(int).Name, numberWidgetType },
            { typeof(float).Name, numberWidgetType },
            { typeof(double).Name, numberWidgetType },
            { typeof(bool).Name, typeof(InputCheckboxWidget) },
            { typeof(DateTime).Name, dtWidgetType },
            { "select", typeof(SelectWidget) },
        };
    }

    public virtual Widget? GetWidget(Type type)
    {
        return GetWidget(type.Name);
    }

    public virtual Widget? GetWidget(string name)
    {
        Type? widgetType = _widgets.GetValueOrDefault(name, null);

        if (widgetType == null)
        {
            return null;
        }

        return Activator.CreateInstance(widgetType) as Widget;
    }

    public virtual Widget? GetWidget(DataType type)
    {
        Type? widgetType = _widgets2.GetValueOrDefault(type, null);

        if (widgetType == null)
        {
            return null;
        }

        return Activator.CreateInstance(widgetType) as Widget;
    }
}
