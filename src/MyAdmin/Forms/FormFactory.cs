using Microsoft.Extensions.DependencyInjection;

namespace MyAdmin.Admin;

public class FormFactory
{
    private readonly IServiceProvider provider;

    public FormFactory(IServiceProvider provider) 
    {
        this.provider = provider;
    }

    public Form Create()
    {
        return provider.GetRequiredService<Form>();
    }
}
