using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyAdmin.Admin.Widgets;
using MyAdmin.Fields;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyAdmin.Admin;

public class Form : IRenderable
{
    private readonly IHttpContextAccessor contextAccessor;

    public Form(IHttpContextAccessor contextAccessor)
    {
        this.contextAccessor = contextAccessor;
    }

    public string Action { get; set; } = "";
    public string Method { get; set; } = "post";
    public Type? ModelType { get; private set; }
    public IFormCollection? Data { get; set; }
    public Dictionary<string, object> CleanedData { get; set; } = new();
    public List<ValidationException> ValidationErrors { get; set; } = new();
    public List<Field> Fields { get; } = new();

    private string GetNameAttributeValue(string name)
    {
        return $"field-{name}";
    }

    public virtual void CreateFields(Type type)
    {
        ModelType = type;
        var widgets = new WidgetFactory();
        var fields = new FieldFactory();
        
        object? instance = Activator.CreateInstance(type);

        foreach (PropertyInfo prop in type.GetProperties())
        {
            Type propType = prop.PropertyType;
            Type? underlyingType = Nullable.GetUnderlyingType(propType);
            if (underlyingType != null)
            {
                propType = underlyingType;
            }

            // Ignore if property is a primary key
            KeyAttribute? keyAttr = prop.GetCustomAttribute<KeyAttribute>();

            if (keyAttr != null || prop.Name == "Id")
            {
                continue;
            }

            Widget? widget;
            DataTypeAttribute? attr = prop.GetCustomAttribute<DataTypeAttribute>();

            // Try get widget by DataTypeAttribute first
            if (attr != null)
            {
                widget = widgets.GetWidget(attr.DataType);
            }
            // If DataTypeAttribute was not found, then try get widget by PropertyType.
            else
            {
                widget = widgets.GetWidget(propType);
            }

            if (widget != null)
            {
                // Check default values
                object? val = prop.GetValue(instance);
                if (val != null)
                {
                    widget.SetValue(val);
                }

                // Check required 
                RequiredAttribute? requiredAttr = prop.GetCustomAttribute<RequiredAttribute>();
                if (requiredAttr != null)
                {
                    widget.SetAttribute("required", "");
                }

                widget.SetAttribute("name", GetNameAttributeValue(prop.Name));
                Field field = fields.GetField(propType.Name, widget, prop)!;

                Fields.Add(field);
            }
        }
    }

    public virtual void SetWidgets(IFormCollection data)
    {
        Data = data;
        foreach (Field field in Fields)
        {
            string key = field.GetWidget.GetAttribute("name");
            if (data.ContainsKey(key))
            {
                field.GetWidget.SetValue(data[key]!);
            }
        }
    }

    public virtual void SetWidgets(object instance)
    {
        foreach (Field field in Fields)
        {
            object? val = field.Property.GetValue(instance);
            if (val != null)
            {
                field.GetWidget.SetValue(val.ToString()!);
            }
        }
    }

    /// <summary>
    /// Validate form and generate CleanedData dictionary
    /// </summary>
    /// <returns></returns>
    public virtual async Task<bool> IsValid()
    {
        // Validate csrf token
        HttpContext httpContext = contextAccessor.HttpContext!;
        var antiForgery = httpContext.RequestServices.GetRequiredService<IAntiforgery>();
        try
        {
            await antiForgery.ValidateRequestAsync(httpContext);
        }
        catch (AntiforgeryValidationException)
        {
            return false;
        }

        // validate fields
        if (Fields.Count < 1)
        {
            return false;
        }

        bool isValid = true;
        foreach (Field field in Fields)
        {
            if (!field.GetWidget.Validate())
            {
                isValid = false;
            }

            try
            {
                field.Validate();
            }
            catch (ValidationException exception)
            {
                ValidationErrors.Add(exception);
                isValid = false;
                continue;
            }

            CleanedData[field.GetWidget.GetAttribute("name")] = field.GetValue()!;
        }

        return isValid;
    }

    public virtual void Save<TContext>(TContext dbContext)
        where TContext : DbContext
    {
        if (ModelType == null || Data == null)
        {
            throw new InvalidOperationException();
        }

        object instance = Activator.CreateInstance(ModelType)!;
        foreach (Field field in Fields)
        {
            PropertyInfo prop = field.Property;
            prop.SetValue(instance, field.GetValue());
        }

        dbContext.Add(instance);
        dbContext.SaveChanges();
    }

    public virtual void Save<TContext>(TContext dbContext, object instance)
        where TContext : DbContext
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(dbContext));
        ArgumentException.ThrowIfNullOrEmpty(nameof(instance));

        if (Data == null)
        {
            throw new InvalidOperationException();
        }

        foreach (Field field in Fields)
        {
            PropertyInfo prop = field.Property;
            prop.SetValue(instance, field.GetValue());
        }

        dbContext.Update(instance);
        dbContext.SaveChanges();
    }

    public virtual string Render()
    {
        string fieldsHtml = "";
        foreach (Field field in Fields)
        {
            fieldsHtml +=
                $$"""
				{{field.Render()}}
				""";
        }

        HttpContext httpContext = contextAccessor.HttpContext!;
        var antiForgery = httpContext.RequestServices.GetRequiredService<IAntiforgery>();
        AntiforgeryTokenSet tokens = antiForgery.GetAndStoreTokens(httpContext);

        return
            $$"""
			<form method="{{Method}}" action="{{Action}}" >
				<input name="{{tokens.FormFieldName}}" type="hidden" value="{{tokens.RequestToken}}">

				{{fieldsHtml}}
				
				<div class="d-flex align-items-end justify-content-end">
					<input type="submit" class="btn btn-primary me-2" value="Save and add another" name="_addanother"/>
					<input type="submit" class="btn btn-primary me-2" value="Save and continue editing" name="_save_continue" />
					<input type="submit" class="btn btn-primary" value="Save" name="_save" />
				</div>
			</form>
			""";
    }
}
