namespace MyAdmin.Admin.Widgets;

public class InputCheckboxWidget : Widget
{
    public InputCheckboxWidget()
    {
        SetAttribute("type", "checkbox");
        SetAttribute("class", "form-check-input");
    }
}
