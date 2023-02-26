using Microsoft.AspNetCore.Http;
using Razor.Templating.Core;

namespace MyAdmin.Admin;

public class RenderResult : IResult
{
    public string TemplatePath { get; set; } = "";

    private Dictionary<string, object> _parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyDictionary<string, object>? Parameters
    {
        get
        {
            _parameters ??= new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            return _parameters.AsReadOnly();
        }
        set
        {
            if (value == null)
            {
                return;
            }
            _parameters = new Dictionary<string, object>(value);
        }
    }

    public object? ViewModel { get; set; }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        httpContext.Response.StatusCode = 200;
        httpContext.Response.ContentType = "text/html; charset=UTF-8";

        var html = await RazorTemplateEngine.RenderAsync(TemplatePath, ViewModel, _parameters);

        httpContext.Response.BodyWriter.WriteHtml(html);
        await httpContext.Response.BodyWriter.FlushAsync().ConfigureAwait(false);
    }
}
