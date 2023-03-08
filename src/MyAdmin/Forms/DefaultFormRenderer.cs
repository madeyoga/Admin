using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MyAdmin.Fields;

namespace MyAdmin.Admin;

public class DefaultFormRenderer : IFormRenderer
{
    public static readonly string Name = "Default";
    private readonly IHttpContextAccessor contextAccessor;

    public DefaultFormRenderer(IHttpContextAccessor contextAccessor)
    {
        this.contextAccessor = contextAccessor;
    }

    public string Render(Form form)
    {
        string fieldsHtml = "";
        foreach (Field field in form.Fields)
        {
            fieldsHtml +=
                $$"""
				{{field.Render()}}
				""";
        }

        HttpContext httpContext = contextAccessor.HttpContext!;
        var antiForgery = httpContext.RequestServices.GetRequiredService<IAntiforgery>();
        AntiforgeryTokenSet tokens = antiForgery.GetAndStoreTokens(httpContext);

        return
            $$"""
			<form method="{{form.Method}}" action="{{form.Action}}" >
				<input name="{{tokens.FormFieldName}}" type="hidden" value="{{tokens.RequestToken}}">

				{{fieldsHtml}}
				
				<div class="d-flex align-items-end justify-content-end">
					<input type="submit" class="btn btn-primary me-2" value="Save and add another" name="_addanother"/>
					<input type="submit" class="btn btn-primary me-2" value="Save and continue editing" name="_save_continue" />
					<input type="submit" class="btn btn-primary" value="Save" name="_save" />
				</div>
			</form>
			""";
    }
}
