namespace MyAdmin.Admin.Widgets;

public class InputTextWidget : Widget
{
    public InputTextWidget()
    {
        SetAttribute("type", "text");
        SetAttribute("class", "form-control");
    }
}
