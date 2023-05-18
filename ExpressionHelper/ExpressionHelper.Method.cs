using System.ComponentModel;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using Util.Ex;

namespace Util.Common
{
    public static partial class ExpressionHelper
    {
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> or)
        {
            if (expr == null)
            {
                return or;
            }

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(new AddExpression(expr.Parameters[0], or.Parameters[0]).Visit(expr.Body), or.Body), or.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> and)
        {
            if (expr == null)
            {
                return and;
            }

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(new AddExpression(expr.Parameters[0], and.Parameters[0]).Visit(expr.Body), and.Body), and.Parameters);
        }
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
        public static Expression<Func<T, bool>> GetCriteriaWhere<T>(string fieldName, Operations selectedOperator, object fieldValue)
        {
            PropertyDescriptor property = GetProperty(TypeDescriptor.GetProperties(typeof(T)), fieldName, ignoreCase: true);
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T));
            MemberExpression memberExpression = GetMemberExpression(parameterExpression, fieldName);
            if (property != null && fieldValue != null)
            {
                Operations operationExpression = selectedOperator;
                if ((property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?)) && selectedOperator != 0)
                {
                    operationExpression = Operations.Equals;
                }

                Type conversionType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var typeConvert = property.PropertyType == typeof(Guid) ? Guid.Parse(fieldValue.ToString()) : Convert.ChangeType(fieldValue, conversionType);
                switch (operationExpression)
                {
                    case Operations.Equals:
                        return Expression.Lambda<Func<T, bool>>(Expression.Equal(memberExpression, Expression.Constant(typeConvert, property.PropertyType)), new ParameterExpression[1] { parameterExpression });
                    case Operations.NotEquals:
                        return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(memberExpression, Expression.Constant(typeConvert, property.PropertyType)), new ParameterExpression[1] { parameterExpression });
                    case Operations.Minor:
                        return Expression.Lambda<Func<T, bool>>(Expression.LessThan(memberExpression, Expression.Constant(typeConvert, property.PropertyType)), new ParameterExpression[1] { parameterExpression });
                    case Operations.MinorEquals:
                        return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(memberExpression, Expression.Constant(typeConvert, property.PropertyType)), new ParameterExpression[1] { parameterExpression });
                    case Operations.Mayor:
                        return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(memberExpression, Expression.Constant(typeConvert, property.PropertyType)), new ParameterExpression[1] { parameterExpression });
                    case Operations.MayorEquals:
                        return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(memberExpression, Expression.Constant(typeConvert, property.PropertyType)), new ParameterExpression[1] { parameterExpression });
                    case Operations.Like:
                        {
                            MethodInfo method6 = typeof(string).GetMethod("Contains", new Type[1] { typeof(string) });
                            return Expression.Lambda<Func<T, bool>>(ExpressionMethodString(property, fieldValue, memberExpression, method6), new ParameterExpression[1] { parameterExpression });
                        }
                    case Operations.NotLike:
                        {
                            MethodInfo method5 = typeof(string).GetMethod("Contains", new Type[1] { typeof(string) });
                            return Expression.Lambda<Func<T, bool>>(Expression.Not(ExpressionMethodString(property, fieldValue, memberExpression, method5)), new ParameterExpression[1] { parameterExpression });
                        }
                    case Operations.StartsWith:
                        {
                            MethodInfo method4 = typeof(string).GetMethod("StartsWith", new Type[1] { typeof(string) });
                            return Expression.Lambda<Func<T, bool>>(ExpressionMethodString(property, fieldValue, memberExpression, method4), new ParameterExpression[1] { parameterExpression });
                        }
                    case Operations.NotStartsWith:
                        {
                            MethodInfo method3 = typeof(string).GetMethod("StartsWith", new Type[1] { typeof(string) });
                            return Expression.Lambda<Func<T, bool>>(Expression.Not(ExpressionMethodString(property, fieldValue, memberExpression, method3)), new ParameterExpression[1] { parameterExpression });
                        }
                    case Operations.EndsWith:
                        {
                            MethodInfo method2 = typeof(string).GetMethod("EndsWith", new Type[1] { typeof(string) });
                            return Expression.Lambda<Func<T, bool>>(ExpressionMethodString(property, fieldValue, memberExpression, method2), new ParameterExpression[1] { parameterExpression });
                        }
                    case Operations.NotEndsWith:
                        {
                            MethodInfo method = typeof(string).GetMethod("EndsWith", new Type[1] { typeof(string) });
                            return Expression.Lambda<Func<T, bool>>(Expression.Not(ExpressionMethodString(property, fieldValue, memberExpression, method)), new ParameterExpression[1] { parameterExpression });
                        }
                    case Operations.Contains:
                        return Contains<T>(fieldValue, parameterExpression, memberExpression);
                    default:
                        throw new DomainException("Error General - Not implement Operation");
                }
            }

            return (T x) => true;
        }
        private static PropertyDescriptor GetProperty(PropertyDescriptorCollection props, string fieldName, bool ignoreCase)
        {
            if (!fieldName.Contains('.'))
            {
                return props.Find(fieldName, ignoreCase);
            }

            var fieldNameProperty = fieldName.Split('.');
            var properties = props.Find(fieldNameProperty[0], ignoreCase);
            for (int i = 1; i < fieldNameProperty.Count(); i++)
            {
                properties = properties.GetChildProperties().Find(fieldNameProperty[i], ignoreCase);
            }

            return properties;
        }
        private static MethodCallExpression ExpressionMethodString(PropertyDescriptor prop, object fieldValue,
                                                                   MemberExpression memberExpression, MethodInfo methodInfo)
        {
            MethodCallExpression bodyLike;
            if (typeof(string) != prop.PropertyType)
            {
                var toString = typeof(Object).GetMethod("ToString");
                var toStringValue = Expression.Call(memberExpression, toString);

                bodyLike = Expression.Call(toStringValue, methodInfo, Expression.Constant(fieldValue, typeof(string)));
            }
            else
            {
                bodyLike = Expression.Call(memberExpression, methodInfo, Expression.Constant(fieldValue, prop.PropertyType));
            }

            return bodyLike;
        }

        private static Expression<Func<T, bool>> Contains<T>(object fieldValue, ParameterExpression parameterExpression, MemberExpression memberExpression)
        {
            var list = (List<long>)fieldValue;

            if (list == null || list.Count == 0)
            {
                return x => true;
            }

            MethodInfo containsInList = typeof(List<long>).GetMethod("Contains", new Type[] { typeof(long) });
            var bodyContains = Expression.Call(Expression.Constant(fieldValue), containsInList, memberExpression);
            return Expression.Lambda<Func<T, bool>>(bodyContains, parameterExpression);
        }
    }
}
