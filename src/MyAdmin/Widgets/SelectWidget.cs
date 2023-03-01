using System.Text;
using System.Web;

namespace MyAdmin.Admin.Widgets;
public class SelectWidget : Widget
{
    private readonly List<SelectOption> selectOptions = new();

    public SelectWidget()
    {
        tag = "select";
        SetAttribute("class", "form-select");
    }

    public override string Render()
    {
        var builder = new StringBuilder();
        foreach (var option in selectOptions)
        {
            builder.AppendLine($"""<{tag} value="{HttpUtility.HtmlEncode(option.Value)}">{HttpUtility.HtmlEncode(option.Label)}</{tag}>""");
        }

        string html = $$"""
            <{{tag}} {{RenderAttributes()}}>
                {{builder.ToString()}}
            </{{tag}}>
            """;

        return html;
    }
}

internal record SelectOption(string Value, string Label);
