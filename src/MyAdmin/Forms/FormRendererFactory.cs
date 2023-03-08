using Microsoft.Extensions.DependencyInjection;

namespace MyAdmin.Admin;

public class FormRendererFactory
{
    private readonly IServiceProvider serviceProvider;
    private readonly Dictionary<string, Type> renderers = new Dictionary<string, Type>();

    public FormRendererFactory(IServiceProvider serviceProvider) 
    {
        this.serviceProvider = serviceProvider;

        renderers.Add("Default", typeof(DefaultFormRenderer));
        renderers.Add("Tabular", typeof(TabularInlineFormRenderer));
        renderers.Add("Stacked", typeof(StackedInlineFormRenderer));
    }

    public IFormRenderer GetFormRenderer(string name)
    {
        if (!renderers.ContainsKey(name))
        {
            throw new KeyNotFoundException(name);
        }
        Type rendererType = renderers.GetValueOrDefault(name)!;
        return (serviceProvider.GetRequiredService(rendererType) as IFormRenderer)!;
    }
}
