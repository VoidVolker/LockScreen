using System;
using System.Linq.Expressions;

namespace LockScreen.Extension.Properties
{
    //https://habr.com/ru/articles/149835/
    public static class ExpressionExtensions
    {
        public static string RetrieveMemberName<TArg, TRes>(this Expression<Func<TArg, TRes>> propertyExpression)
        {
            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                if (propertyExpression.Body is UnaryExpression unaryExpression)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
            }
            if (memberExpression != null)
            {
                if (
                    memberExpression.Expression is ParameterExpression parameterExpression
                    && parameterExpression.Name == propertyExpression.Parameters[0].Name)
                {
                    return memberExpression.Member.Name;
                }
            }
            throw new ArgumentException("Invalid expression.", nameof(propertyExpression));
        }
    }
}
