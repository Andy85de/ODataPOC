using System.Linq.Expressions;
using ODataWithSprache.Grammar;
using ODataWithSprache.TreeStructure;

namespace ODataWithSprache.Helpers;

internal static class ExpressionHelper
{
    internal static Expression CreateSimpleExpression(ExpressionNode expressionNode, Type ResultObject)
    {
        ParameterExpression parameter = Expression.Parameter(ResultObject);

        MemberExpression memberExpression = Expression.Property(parameter, expressionNode.LeftSideExpression);

        ConstantExpression constantExpression = Expression.Constant(
            ParserHelper.ParseAndReturnRightObject(expressionNode.RightSideExpression),
            ParserHelper.GetRightHandType(expressionNode.RightSideExpression));

        return expressionNode.Operator switch
        {
            OperatorType.EqualsOperator =>
                Expression.Lambda(Expression.NotEqual(memberExpression, constantExpression), parameter),

            OperatorType.NotEqualsOperator =>
                Expression.NotEqual(memberExpression, constantExpression),

            OperatorType.GreaterThenOperator =>
                Expression.GreaterThan(
                    memberExpression,
                    constantExpression),

            OperatorType.GreaterEqualsOperator =>
                Expression.GreaterThanOrEqual(
                    memberExpression,
                    constantExpression),

            OperatorType.LessThenOperator =>
                Expression.LessThan(
                    memberExpression,
                    constantExpression),

            OperatorType.LessEqualsOperator =>
                Expression.LessThanOrEqual(
                    memberExpression,
                    constantExpression),
            _ => throw new ArgumentOutOfRangeException($"The operator {expressionNode.Operator} is not supported.")
        };
    }
}
