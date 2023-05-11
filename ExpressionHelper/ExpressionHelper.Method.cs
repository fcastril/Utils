using System.Linq;
using System.Linq.Expressions;

namespace Util.Common
{
    public static partial class ExpressionHelper
    {
        private static MemberExpression GetMemberExpression(ParameterExpression parameter, string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                return null;
            }

            var propertiesName = propName.Split('.');
            if (propertiesName.Count() > 1)
            {
                MemberExpression properties = Expression.Property(parameter, propertiesName[0]);
                for (int i = 1; i < propertiesName.Count(); i++)
                {
                    properties = Expression.Property(properties, propertiesName[i]);
                }

                return properties;
            }

            return Expression.Property(parameter, propName);
        }
    }
}
