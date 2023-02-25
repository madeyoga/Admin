namespace MyAdmin.Admin.Widgets;

public class InputNumberWidget : Widget
{
    public InputNumberWidget()
    {
        SetAttribute("type", "number");
        SetAttribute("class", "form-control");
    }

	public override object? GetValue()
	{
		return int.Parse((base.GetValue() as string)!);
	}

	public override bool ValidateValue()
	{
		try
		{
			int.Parse(GetAttribute("value"));
		}
		catch(Exception) // Argument null / invalid format / overflow
		{
			return false;
		}
		return true;
	}
}
