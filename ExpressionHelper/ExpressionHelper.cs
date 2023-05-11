using System.Linq;
using System.Linq.Expressions;
using Util.Common;

namespace Util.Common
{
    public static partial class ExpressionHelper
    {
        public static MethodCallExpression OrderBy<T>(this IQueryable<T> query,
                                                      string orderByMember,
                                                      DirecOrder direction)
        {
            return OrderBy(query, orderByMember, direction, "OrderBy");
        }

        public static MethodCallExpression ThenBy<T>(this IQueryable<T> query,
                                                      string orderByMember,
                                                      DirecOrder direction)
        {
            return OrderBy(query, orderByMember, direction, "ThenBy");
        }

        private static MethodCallExpression OrderBy<T>(this IQueryable<T> query,
                                                          string orderByMember,
                                                          DirecOrder direction,
                                                          string initOrder)
        {
            if (query.IsNotNull() && orderByMember.IsValid())
            {
                var queryElementTypeParam = Expression.Parameter(typeof(T));
                var memberAccess = GetMemberExpression(queryElementTypeParam, orderByMember);
                var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

                var orderBy = Expression.Call(
                                            typeof(Queryable),
                                            direction == DirecOrder.Ascending ? initOrder : initOrder + "Descending",
                                            new[] { typeof(T), memberAccess.Type },
                                            query.Expression,
                                            Expression.Quote(keySelector));

                return orderBy;
            }

            return null;
        }
    }
}
