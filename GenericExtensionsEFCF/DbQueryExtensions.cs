using System;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace GenericExtensionsEFCF
{
    public static class DbQueryExtensions
    {

        public static DbQuery<T> Include<T>(this DbQuery<T> dbSet, Expression<Func<T, object>> selector) where T : class
        {
            string propertyName = GetPropertyName(selector);
            return dbSet.Include(propertyName);
        }

        private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            var memberExpr = expression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException("Expression body must be a member expression");
            return memberExpr.Member.Name;
        }
    }
}