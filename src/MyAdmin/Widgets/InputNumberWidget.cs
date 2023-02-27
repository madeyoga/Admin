namespace MyAdmin.Admin.Widgets;

public class InputNumberWidget : Widget
{
    public InputNumberWidget()
    {
        SetAttribute("type", "number");
        SetAttribute("class", "form-control");
    }
}
