namespace MyAdmin.Admin.Widgets;

public class TextareaWidget : Widget
{
    private string? _value;

    public TextareaWidget()
    {
        tag = "textarea";
        SetAttribute("class", "form-control");
    }

    public override void SetValue(string value)
    {
        _value = value;
    }

    public override string? GetValue()
    {
        return _value;
    }

    public override string Render()
    {
        return $"<{tag} {RenderAttributes()} >{_value}</{tag}>";
    }
}
