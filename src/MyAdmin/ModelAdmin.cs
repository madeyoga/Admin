using Microsoft.AspNetCore.Http;

namespace MyAdmin.Admin;

public class ModelAdmin
{
    private readonly Form form;

    public Type? ModelType { get; set; }
    public string Index_Template { get; set; } = "~/Areas/Admin/ModelIndex.cshtml";
    public string FormAdd_Template { get; set; } = "~/Areas/Admin/FormAdd_Template.cshtml";
    public string FormChange_Template { get; set; } = "~/Areas/Admin/FormChange_Template.cshtml";
    public List<string> ListDisplay { get; private set; } = new List<string>();

    public ModelAdmin(Form form)
    {
        this.form = form;
    }

    public Form GetForm()
    {
        form.CreateFields(ModelType!);
        return form;
    }

    public Form GetForm(IFormCollection data)
    {
        var form = GetForm();
        form.SetWidgets(data);
        return form;
    }
}
