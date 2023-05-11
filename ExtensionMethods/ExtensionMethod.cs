using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Util   .Common
{
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ValidatedNotNullAttribute: Attribute { }
    public static partial class ExtensionMethod
    {

        #region IsNotNull
        public static bool IsNotNull<T>([ValidatedNotNull] this T valid) where T : class => valid != null;
        public static bool IsNotNull<T>([ValidatedNotNull] this List<T> valid) where T : class => (valid != null && valid.Any());

        #endregion
        #region IsValid
        public static bool IsValid([ValidatedNotNull] this string s) => (s.IsNotNull() && s.Trim().Length > 0);

        #endregion
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query,
                                                      List<OrderPaginate> orders)
        {
            if (orders.IsNotNull())
            {
                MethodCallExpression OrderBy;
                for (int i = 0; i < orders.Count; i++)
                {
                    if (!System.Enum.TryParse(orders[i].Direction.FirstCharToUpper(), out DirecOrder eOrden))
                    {
                        eOrden = DirecOrder.Ascending;
                    }

                    OrderBy = (i == 0) ?
                            
                                ExpressionHelper.OrderBy(query, orders[i].Name, eOrden) :
                                ExpressionHelper.ThenBy(query, orders[i].Name, eOrden);

                    if (OrderBy.IsNotNull())
                    {
                        query = query.Provider.CreateQuery<T>(OrderBy);
                    }
                }
            }

            return query;
        }



        public static string FirstCharToUpper(this string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return char.ToUpper(s[0], CultureInfo.CurrentCulture) + s[1..].ToLower(CultureInfo.CurrentCulture);
        }
    }
}
