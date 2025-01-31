﻿using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using MyAdmin.Admin.Widgets;
using MyAdmin.Fields;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MyAdmin.Admin;

public class Form : IRenderable
{
    private readonly IHttpContextAccessor contextAccessor;
    private readonly RouteHelper routeHelper;
    private readonly IFieldService fieldService;

    public Form(IHttpContextAccessor contextAccessor, RouteHelper routeHelper, IFieldService fieldService)
    {
        this.contextAccessor = contextAccessor;
        this.routeHelper = routeHelper;
        this.fieldService = fieldService;
    }

    public string Action { get; set; } = "";
    public string Method { get; set; } = "post";
    public Type? ModelType { get; private set; }
    public IFormCollection? Data { get; set; }
    public Dictionary<string, object> CleanedData { get; private set; } = new();
    public List<ValidationException> ValidationErrors { get; private set; } = new();
    public List<ValidationResult> ValidationResults { get; private set; } = new();
    public List<Field> Fields { get; } = new();
    public IFormRenderer? Renderer { get; set; }

    private string GetNameAttributeValue(string name)
    {
        return $"field-{name}";
    }

    public virtual void CreateFields(Type type)
    {
        ModelType = type;

        IEntityType? entityType = fieldService.FindEntityType(type);
        if (entityType == null)
        {
            throw new InvalidOperationException($"Unable to find {type.Name} in DbContext");
        }

        var widgets = new WidgetFactory();
        var fields = new FieldFactory();

        object? instance = Activator.CreateInstance(type)!;
        EntityEntry entityEntry = fieldService.CreateEntityEntry(instance);

        foreach (IProperty property in entityType.GetProperties())
        {
            if (property.PropertyInfo != null && property.PropertyInfo!.IsDefined(typeof(NotMappedAttribute)))
            {
                continue;
            }

            if (property.IsKey())
            {
                continue;
            }

            if (property.IsForeignKey())
            {
                Widget? widget = widgets.GetWidget("select")!;
                widget.AppendAttribute("class", "field-foreignkey");

                IProperty fkProperty = property;
                INavigation? navigationProperty;

                if (property.IsShadowProperty())
                {
                    string navigationPropertyName = fkProperty.Name.Substring(0, fkProperty.Name.Length - 2);
                    navigationProperty = entityType.FindNavigation(navigationPropertyName);
                }
                else
                {
                    navigationProperty = entityType.FindNavigation(property.Name);
                }

                if (navigationProperty == null)
                {
                    foreach (INavigation navProp in entityType.GetNavigations())
                    {
                        if (navProp.ForeignKey.Properties.Contains(property))
                        {
                            navigationProperty = navProp;
                            break;
                        }
                    }
                }

                widget.SetAttribute("id", $"id-{GetNameAttributeValue(fkProperty.Name)}");
                widget.SetAttribute("name", GetNameAttributeValue(fkProperty.Name));

                string fetchUrl = routeHelper.Reverse("MyAdmin_FetchData_Get", new 
                {
                    modelName = navigationProperty!.ClrType.Name,
                });
                widget.SetAttribute("data-fetch-url", fetchUrl);

                IKey? key = navigationProperty.TargetEntityType.FindPrimaryKey();
                if (key == null)
                {
                    throw new InvalidOperationException();
                }
                string? pkName = key.Properties.Single().Name;

                if (pkName == null)
                {
                    throw new InvalidOperationException($"Unable to find primary from entity type {navigationProperty.Name}");
                }
                widget.SetAttribute("data-pk-name", pkName);
                widget.SetAttribute("data-model-name", navigationProperty.ClrType.Name);

                object? val = entityEntry.Property(fkProperty).CurrentValue; // fkProp.GetValue(instance);
                if (val != null)
                {
                    widget.SetAttribute("value", val.ToString()!);
                }

                Type? propType = fkProperty.ClrType;
                Type? underlyingType = Nullable.GetUnderlyingType(fkProperty.ClrType);
                if (underlyingType != null)
                {
                    propType = underlyingType;
                }

                Field? field = fields.GetField(propType.Name, widget, fkProperty);
                if (field != null)
                {
                    Fields.Add(field);
                }
            }
            else
            {
                PropertyInfo propertyInfo = property.PropertyInfo!;

                Type? propType = propertyInfo.PropertyType;
                Type? underlyingType = Nullable.GetUnderlyingType(propType);
                if (underlyingType != null)
                {
                    propType = underlyingType;
                }

                Widget? widget;
                DataTypeAttribute? attr = propertyInfo.GetCustomAttribute<DataTypeAttribute>();

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
                    object? val = propertyInfo.GetValue(instance);
                    if (val != null)
                    {
                        widget.SetValue(val);
                    }

                    // Check required 
                    RequiredAttribute? requiredAttr = propertyInfo.GetCustomAttribute<RequiredAttribute>();
                    if (requiredAttr != null)
                    {
                        widget.SetAttribute("required", "");
                    }

                    widget.SetAttribute("name", GetNameAttributeValue(propertyInfo.Name));
                    Field? field = fields.GetField(propType.Name, widget, property);

                    if (field != null)
                    {
                        Fields.Add(field);
                    }
                }
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

        bool isValidDatatype = true;
        object instance = Activator.CreateInstance(ModelType!)!;
        foreach (Field field in Fields)
        {
            if (!field.GetWidget.Validate())
            {
                isValidDatatype = false;
            }

            try
            {
                field.Validate();
            }
            catch (ValidationException exception)
            {
                ValidationErrors.Add(exception);
                isValidDatatype = false;
                continue;
            }

            object value = field.GetValue()!;
            CleanedData[field.GetWidget.GetAttribute("name")] = value;
            if (field.Property != null)
            {
                Console.WriteLine(value);
                field.Property.SetValue(instance, value);
            }
        }

        ValidationContext validationContext = new ValidationContext(instance);

        bool isValidAnnotations = Validator.TryValidateObject(instance, validationContext, ValidationResults, true);

        return isValidDatatype && isValidAnnotations;
    }

    public virtual void Save<TContext>(TContext dbContext, bool commit = true)
        where TContext : DbContext
    {
        if (ModelType == null || Data == null)
        {
            throw new InvalidOperationException();
        }

        object instance = Activator.CreateInstance(ModelType)!;
        EntityEntry entry = fieldService.CreateEntityEntry(instance);
        foreach (Field field in Fields)
        {
            IProperty property = field.EntityProperty;
            entry.Property(property).CurrentValue = field.GetValue();
        }

        dbContext.Add(instance);
        if (commit)
        {
            dbContext.SaveChanges();
        }
    }

    public virtual void Save<TContext>(TContext dbContext, object instance, bool commit = true)
        where TContext : DbContext
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(dbContext));
        ArgumentException.ThrowIfNullOrEmpty(nameof(instance));

        if (Data == null)
        {
            throw new InvalidOperationException();
        }

        EntityEntry entry = fieldService.CreateEntityEntry(instance);
        foreach (Field field in Fields)
        {
            IProperty property = field.EntityProperty;
            entry.Property(property).CurrentValue = field.GetValue();
        }

        dbContext.Update(instance);
        if (commit)
        {
            dbContext.SaveChanges();
        }
    }

    public virtual string Render()
    {
        if (Renderer == null)
        {
            throw new InvalidOperationException("Unable to render form. Form renderer is null");
        }
        return Renderer.Render(this);
    }
}
