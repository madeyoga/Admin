using Microsoft.EntityFrameworkCore.Metadata;
using MyAdmin.Admin;
using MyAdmin.Admin.Widgets;
using System.Reflection;

namespace MyAdmin.Fields;

public abstract class Field : IRenderable
{
    public string Label = "";
    public string Hint = "";
    public Widget GetWidget { get; set; }
    public PropertyInfo Property { get; set; }
    public IProperty EntityProperty { get; set; }

    public Field(Widget widget, PropertyInfo property)
    {
        Label = property.Name;
        GetWidget = widget;
        Property = property;
    }

    public Field(Widget widget, IProperty property)
    {
        Label = property.Name;
        GetWidget = widget;
        EntityProperty = property;
        if (property.PropertyInfo != null)
        {
            Property = property.PropertyInfo;
        }
    }

    /// <summary>
    /// Try convert the value to a correct datatype and throw ValidationException if that is not possible
    /// </summary>
    public abstract void Validate();

    /// <summary>
    /// Get the validated value
    /// </summary>
    /// <returns>This field value with a correct datatype</returns>
    public abstract object? GetValue();

    public string Render()
    {
        string html =
            $$"""
			<div class="mb-3">
				<label for="{{GetWidget.GetAttribute("name")}}" class="form-label">{{Label}}</label>
				{{GetWidget.Render()}}
			</div>
			""";
        return html;
    }
}
