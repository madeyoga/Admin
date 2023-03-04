using System.Linq.Expressions;
using System.Reflection;

namespace MyAdmin;

public static class QueryableExtensions
{
    public static IQueryable<object>? OrderAscending(this IQueryable<object> source, Type entityType, PropertyInfo property)
    {
        ParameterExpression parameterExpression = Expression.Parameter(entityType);
        Expression propertyExpression = Expression.Property(parameterExpression, property);
        LambdaExpression lambdaExpression = Expression.Lambda(propertyExpression, parameterExpression);

        MethodInfo orderByMethod = typeof(Queryable).GetMethods().Single(
            method => method.Name == "OrderBy" && method.GetParameters().Length == 2
        );

        MethodInfo orderByGenericMethod = orderByMethod.MakeGenericMethod(entityType, propertyExpression.Type);

        object? orderedQuery = orderByGenericMethod.Invoke(null, new object[] { source, lambdaExpression });

        return orderedQuery as IQueryable<object>;
    }

    public static IQueryable<object>? OrderDescending(this IQueryable<object> source, Type entityType, PropertyInfo property)
    {
        ParameterExpression parameterExpression = Expression.Parameter(entityType);
        Expression propertyExpression = Expression.Property(parameterExpression, property);
        LambdaExpression lambdaExpression = Expression.Lambda(propertyExpression, parameterExpression);

        MethodInfo orderByMethod = typeof(Queryable).GetMethods().Single(
            method => method.Name == "OrderByDescending" && method.GetParameters().Length == 2
        );

        MethodInfo orderByGenericMethod = orderByMethod.MakeGenericMethod(entityType, propertyExpression.Type);

        object? orderedQuery = orderByGenericMethod.Invoke(null, new object[] { source, lambdaExpression });

        return orderedQuery as IQueryable<object>;
    }
}
