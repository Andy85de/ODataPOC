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

        return expression.BinaryType switch
        {
            ExpressionCombinator.And => Expression.AndAlso(
                VisitNode(expression.LeftChild, optionalParameter),
                VisitNode(expression.RightChild, optionalParameter)),
            ExpressionCombinator.Or => Expression.OrElse(
                VisitNode(expression.LeftChild, optionalParameter),
                VisitNode(expression.RightChild, optionalParameter)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected override Expression VisitExpressionNode(ExpressionNode? property, params object[] optionalParameter)
    {
        if (property == null)
        {
            throw new ArgumentException(nameof(property));
        }

        return ExpressionHelper.CreateSimpleExpression(
            property,
            optionalParameter.OfType<Type>().Single(),
            optionalParameter.OfType<ParameterExpression>().Single());
    }

    protected override Expression VisitRoot(RootNode root, params object[] optionalParameter)
    {
        if (root == null)
        {
            throw new ArgumentNullException(nameof(root));
        }

        Type type = optionalParameter.OfType<Type>().Single();

        ParameterExpression parameter = Expression.Parameter(type);

        Expression result = VisitNode(root, optionalParameter.Append(parameter).ToArray());

        return Expression.Lambda(typeof(Func<,>).MakeGenericType(type, typeof(bool)), result, parameter);
    }

    public override Expression<Func<TModel, bool>> Visit<TModel>(TreeNode? root)
    {
        if (root is not RootNode rootNode)
        {
            throw new ArgumentException($"Root not of type {nameof(RootNode)}.");
        }
        return (Expression<Func<TModel, bool>>) VisitRoot(rootNode, typeof(TModel));
    }
}
