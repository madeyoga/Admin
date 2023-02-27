using System.Web;

namespace MyAdmin.Admin.Widgets;

public class Widget : IRenderable
{
    protected Dictionary<string, string> _attributes = new();
    protected string tag = "input";

    public virtual void SetAttribute(string name, string value)
    {
        _attributes[name] = value;
    }

    public virtual string GetAttribute(string name)
    {
        return _attributes[name];
    }

    public virtual void SetValue(string value)
    {
        SetAttribute("value", value);
    }

    public virtual void SetValue(object value)
    {
        SetValue(value.ToString()!);
    }

    public virtual string? GetValue()
    {
        if (_attributes.TryGetValue("value", out string? value))
        {
            return value;
        }
        return null;
    }

    public virtual bool Validate()
    {
        string? required = _attributes.GetValueOrDefault("requried");

        // if required is present then value cannot be null or empty
        if (required != null)
        {
            string? val = _attributes.GetValueOrDefault("value");
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
        }

        return true;
    }

    public virtual string Render()
    {
        return $"<{tag} {RenderAttributes()} />";
    }

    protected virtual string RenderAttributes()
    {
        string result = "";
        foreach (string key in _attributes.Keys)
        {
            string attr = $"{key}=\"{HttpUtility.HtmlEncode(_attributes[key])}\" ";
            result += attr;
        }
        return result;
    }
}
