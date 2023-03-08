using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyAdmin.Attributes;
using System.Reflection;

namespace MyAdmin.Admin;

public class ModelAdmin
{
    public Type? ModelType { get; set; }
    public string Index_Template { get; set; } = "~/Areas/Admin/ModelIndex.cshtml";
    public string FormAdd_Template { get; set; } = "~/Areas/Admin/FormAdd_Template.cshtml";
    public string FormChange_Template { get; set; } = "~/Areas/Admin/FormChange_Template.cshtml";
    public List<string> ListDisplay { get; private set; } = new List<string>();

    private Form? form;
    private readonly List<Form> Inlines = new List<Form>();
    private readonly List<Type> InlineTypes = new List<Type>();
    private readonly FormBuilder builder;
    private readonly FormRendererFactory formRendererFactory;

    public ModelAdmin(FormBuilder builder, FormRendererFactory formRendererFactory)
    {
        this.builder = builder;
        this.formRendererFactory = formRendererFactory;
    }

    public void CreateForms()
    {
        if (ModelType == null)
        {
            throw new InvalidOperationException("Unable to create Form with null ModelType");
        }

        builder.SetRenderer(formRendererFactory.GetFormRenderer("Default"));
        builder.SetModelType(ModelType);
        form = builder.Create();

        CreateInlineForms();
    }

    public Form GetForm()
    {
        return form!;
    }

    public Form GetForm(IFormCollection data)
    {
        var form = GetForm();
        form.SetWidgets(data);
        return form;
    }

    private void CreateInlineForms()
    {
        foreach (PropertyInfo property in ModelType!.GetProperties())
        {
            if (!property.PropertyType.IsArray)
            {
                continue;
            }

            // if property is a List of navigation properties
            IFormRenderer renderer;
            if (property.IsDefined(typeof(TabularInlineAttribute), false))
            {
                renderer = formRendererFactory.GetFormRenderer("Tabular");
            }
            else if (property.IsDefined(typeof(StackedInlineAttribute), false))
            {
                renderer = formRendererFactory.GetFormRenderer("Stacked");
            }
            else
            {
                continue;
            }

            Type entityType = property.PropertyType.GetGenericArguments().Single();

            builder.SetModelType(entityType);
            builder.SetRenderer(renderer);
            Form inlineForm = builder.Create();

            Inlines.Add(inlineForm);
            InlineTypes.Add(entityType);
        }
    }
}
