using System.Linq.Expressions;
using System.Reflection;
using JasperFx.Core;
using ODataWithSprache.Grammar;
using ODataWithSprache.TreeStructure;

namespace ODataWithSprache.Helpers;

internal static class ExpressionHelper
{
    internal static Expression CreateSimpleExpression(ExpressionNode expressionNode, Type resultObject, ParameterExpression parameter)
    {
        PropertyInfo propertyInfo = resultObject.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .First(p => p.Name.Equals(expressionNode.LeftSideExpression, StringComparison.OrdinalIgnoreCase));

        var propertyExpression = Expression.Property(parameter, propertyInfo);

        ConstantExpression constExpl;

        if (propertyInfo.PropertyType == typeof(string))
        {
            constExpl = Expression.Constant(expressionNode.RightSideExpression);
        }

        else if (propertyInfo.PropertyType == typeof(int))
        {
            constExpl = Expression.Constant(ParserHelper.ParseAndReturnRightObject(expressionNode.RightSideExpression));
        }

        else
        {
            throw new NotImplementedException();
        }
        
        return expressionNode.Operator switch
        {
            OperatorType.EqualsOperator =>
                Expression.Equal(propertyExpression, constExpl),

            OperatorType.NotEqualsOperator =>
                Expression.NotEqual(propertyExpression, constExpl),

            OperatorType.GreaterThenOperator =>
                Expression.GreaterThan(
                    propertyExpression,
                    constExpl),

            OperatorType.GreaterEqualsOperator =>
                Expression.GreaterThanOrEqual(
                    propertyExpression,
                    constExpl),

            OperatorType.LessThenOperator =>
                Expression.LessThan(
                    propertyExpression,
                    constExpl),

            OperatorType.LessEqualsOperator =>
                Expression.LessThanOrEqual(
                    propertyExpression,
                    constExpl),
            
            OperatorType.Contains => CreateContainsExpression(
                propertyExpression,
                constExpl),
                
            _ => throw new ArgumentOutOfRangeException($"The operator {expressionNode.Operator} is not supported.")
        };
    }

    private static Expression CreateContainsExpression(
        MemberExpression memberExpression,
        ConstantExpression constExp)
    {
        MethodInfo? containsMethode = typeof(string).GetMethod(
            nameof(string.Contains),
            BindingFlags.Public | BindingFlags.Instance,
            null,
            new[] { typeof(string), typeof(StringComparison) },
            null);

        return Expression.Call(
            memberExpression,
            containsMethode,
            constExp,
            Expression.Constant(StringComparison.OrdinalIgnoreCase));
    }
}
