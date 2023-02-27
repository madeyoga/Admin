namespace MyAdmin.Admin.Widgets;

public class DateTimeWidget : Widget
{
    public DateTimeWidget()
    {
        SetAttribute("type", "datetime-local");
        SetAttribute("class", "form-control");
    }

    public override void SetValue(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        DateTime date = DateTime.Parse(value);
        SetAttribute("value", date.ToString("yyyy-MM-dd") + "T" + date.ToString("HH:mm"));
    }
}
