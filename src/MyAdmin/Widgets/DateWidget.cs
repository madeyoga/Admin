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
		DateTime date = DateTime.Parse(value);
		SetAttribute("value", date.ToString("yyyy-MM-dd"));
	}

	public override object? GetValue()
	{
		string? value = (string) base.GetValue()!;
		if (value == null)
		{
			return null;
		}
		return DateTime.Parse(value);
	}

	public override bool ValidateValue()
	{
		try
		{
			DateTime.Parse(GetAttribute("value"));
		}
		catch(FormatException)
		{
			return false;
		}
		return true;
	}
}
