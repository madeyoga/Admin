namespace MyAdmin.Admin.Widgets;

public class DateWidget : Widget
{
    public DateWidget()
    {
        SetAttribute("type", "date");
        SetAttribute("class", "form-control");
    }

    public override void SetValue(string value)
    {
        if (string.IsNullOrEmpty(value)) 
        {
            return;
        }

        DateTime date = DateTime.Parse(value);
        SetAttribute("value", date.ToString("yyyy-MM-dd"));
    }
}
