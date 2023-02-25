using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MyAdmin.Admin;

public static class ResultExtensions
{
    public static IResult Render(this IResultExtensions extensions, string templatePath, object? viewModel = null, object? parameters = null)
    {
        return new RenderResult() 
        { 
            TemplatePath = templatePath, 
            ViewModel = viewModel, 
            Parameters = HtmlHelper.ObjectToDictionary(parameters).AsReadOnly(), 
        };
    }
}
