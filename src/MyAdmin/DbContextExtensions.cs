using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MyAdmin.Admin;

public static class DbContextExtensions
{
    public static IQueryable? Set(this DbContext context, Type t)
    {
        MethodInfo[] methods = context.GetType().GetMethods();
        foreach (MethodInfo method in methods)
        {
            if (method.Name == "Set" && method.IsGenericMethod)
            {
                MethodInfo setMethod = method.MakeGenericMethod(t);
                IQueryable? dbset = setMethod.Invoke(context, null)! as IQueryable;
                return dbset;
            }
        }
        return null;
    }
}
