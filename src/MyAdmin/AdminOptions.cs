namespace MyAdmin.Admin;

public class AdminOptions
{
    public string IndexTemplate { get; set; } = "~/Areas/Admin/Index.cshtml";
    public string SiteName { get; set; } = "Aspnetcore Admin";
    public string RootPath { get; set; } = "/admin/";

    private IDictionary<string, ModelAdminTypePair> _admins = new Dictionary<string, ModelAdminTypePair>();
    public IReadOnlyDictionary<string, ModelAdminTypePair> Admins
    {
        get
        {
            return _admins.AsReadOnly();
        }
    }

    public void Register<TModel, TAdmin>()
        where TModel : class
        where TAdmin : ModelAdmin
    {
        var tModel = typeof(TModel);
        var tAdmin = typeof(TAdmin);
        var pair = new ModelAdminTypePair()
        {
            Admin = tAdmin,
            Model = tModel,
        };

        _admins.Add(tModel.Name!, pair);
    }

    public void Register<TModel>()
        where TModel : class
    {
        var tModel = typeof(TModel);
        var tAdmin = typeof(ModelAdmin);
        var pair = new ModelAdminTypePair()
        {
            Admin = tAdmin,
            Model = tModel,
        };

        _admins.Add(tModel.Name!, pair);
    }
}
