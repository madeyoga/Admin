namespace MyAdmin.Admin;

public interface IFormBuilder
{
    Form Create();
    IFormBuilder SetRenderer(IFormRenderer formRenderer);
    IFormBuilder SetModelType(Type modelType);
}