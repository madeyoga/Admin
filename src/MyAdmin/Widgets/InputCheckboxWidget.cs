namespace MyAdmin.Admin.Widgets;

public class InputCheckboxWidget : Widget
{
    public InputCheckboxWidget()
    {
        SetAttribute("type", "checkbox");
        SetAttribute("class", "form-check-input");
    }

    public override object? GetValue()
    {
        string? value = (string)base.GetValue()!;

        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        if (string.Equals(value, "on", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }
}
