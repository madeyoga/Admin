using Microsoft.Extensions.Options;

namespace MyAdmin.Admin;

public class AdminServiceProvider
{
    private readonly IServiceProvider _services;
    private readonly AdminOptions _options;

    public AdminServiceProvider(IServiceProvider services, IOptions<AdminOptions> options)
    {
        _services = services;
        _options = options.Value;
    }

    /// <summary>
    /// Get admin by model type
    /// </summary>
    /// <param name="modelType"></param>
    /// <returns><see cref="ModelAdmin"/></returns>
    public ModelAdmin? GetModelAdmin(Type modelType)
    {
        return GetModelAdmin(modelType.Name);
    }

    /// <summary>
    /// Get admin by model name
    /// </summary>
    /// <param name="modelName"></param>
    /// <returns><see cref="ModelAdmin"/></returns>
    public ModelAdmin? GetModelAdmin(string modelName)
    {
        ModelAdminTypePair? pair = _options.Admins.GetValueOrDefault(modelName);
        if (pair == null)
        {
            return null;
        }

        var admin = _services.GetService(pair.Admin) as ModelAdmin;
        admin!.ModelType = pair.Model;

        return admin;
    }
}
