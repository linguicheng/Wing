using System;
using System.Linq.Expressions;

namespace Wing.Extensions
{
    public static class ExpressionExtension
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            Expression<Func<T, bool>> expression = UpdateParameter<T>(right, left.Parameters[0]);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, expression.Body), left.Parameters);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            Expression<Func<T, bool>> expression = UpdateParameter<T>(right, left.Parameters[0]);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, expression.Body), right.Parameters);
        }

        private static Expression<Func<T, bool>> UpdateParameter<T>(Expression<Func<T, bool>> exp, ParameterExpression newParameter)
        {
            return Expression.Lambda<Func<T, bool>>(new ParameterUpdateVisitor(exp.Parameters[0], newParameter).Visit(exp.Body),
                new ParameterExpression[] { newParameter });
        }

        private class ParameterUpdateVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _newParameter;

            private readonly ParameterExpression _oldParameter;

            public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node == _oldParameter)
                {
                    return _newParameter;
                }

                return base.VisitParameter(node);
            }
        }
    }
}
