using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyAdmin.Admin;

public class FormBuilder : IFormBuilder
{
    private readonly FormFactory formFactory;
    private IFormRenderer? _formRenderer;
    private Type? _modelType;

    public FormBuilder(FormFactory formFactory)
    {
        this.formFactory = formFactory;
    }

    public IFormBuilder SetRenderer(IFormRenderer formRenderer)
    {
        _formRenderer = formRenderer;
        return this;
    }

    public IFormBuilder SetModelType(Type modelType)
    {
        _modelType = modelType;
        return this;
    }

    public Form Create()
    {
        Form form = formFactory.Create();

        form.Renderer = _formRenderer;

        if (_modelType == null)
        {
            throw new InvalidOperationException("Unable to create a Form with null ModelType");
        }

        form.CreateFields2(_modelType);

        return form;
    }
}
