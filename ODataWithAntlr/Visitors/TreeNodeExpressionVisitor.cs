using System.Linq.Expressions;
using ODataWithSprache.Extensions;
using ODataWithSprache.Grammar;
using ODataWithSprache.Helpers;
using ODataWithSprache.TreeStructure;

namespace ODataWithSprache.Visitors;

public class TreeNodeExpressionVisitor: TreeNodeVisitorBase<Expression>
{
    protected override Expression VisitBinaryExpressionNode(BinaryExpressionNode expression, params object[] optionalParameter)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }

        if (expression.RightChild.GetType() == typeof(ExpressionNode))
        {
            return expression.BinaryType switch
            {
                ExpressionCombinator.And => Expression.And(
                    VisitExpressionNode(expression.LeftChild.ToExpressionNode(), optionalParameter),
                    VisitExpressionNode(expression.RightChild.ToExpressionNode(), optionalParameter)),
                ExpressionCombinator.Or => Expression.Or(
                    VisitExpressionNode(expression.LeftChild.ToExpressionNode(), optionalParameter),
                    VisitExpressionNode(expression.RightChild.ToExpressionNode(), optionalParameter)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return expression.BinaryType switch
        {
            ExpressionCombinator.And => Expression.And(
                VisitExpressionNode(expression.LeftChild.ToExpressionNode(), optionalParameter),
                VisitBinaryExpressionNode(expression.RightChild.ToBinaryExpressionNode(), optionalParameter)),
            ExpressionCombinator.Or => Expression.Or(
                VisitExpressionNode(expression.LeftChild.ToExpressionNode(), optionalParameter),
                VisitBinaryExpressionNode(expression.RightChild.ToBinaryExpressionNode(), optionalParameter)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected override Expression VisitExpressionNode(ExpressionNode? property, params object[] optionalParameter)
    {
        if (property == null)
        {
            throw new ArgumentException(nameof(property));
        }

        return ExpressionHelper.CreateSimpleExpression(property, (Type)optionalParameter[0]);
    }

    public override Expression Visit(RootNode root, params object[] optionalParameter)
    {
        if (root == null)
        {
            throw new ArgumentNullException(nameof(root));
        }

        if (root.LeftChild.GetType() == typeof(ExpressionNode))
        {
            return VisitExpressionNode(root.LeftChild.ToExpressionNode(), optionalParameter);
        }

        return VisitBinaryExpressionNode(root.LeftChild.ToBinaryExpressionNode(), optionalParameter);
    }
}
