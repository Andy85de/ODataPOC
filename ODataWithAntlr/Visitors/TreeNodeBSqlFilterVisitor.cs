using ODataWithSprache.Extensions;
using ODataWithSprache.Helpers;
using ODataWithSprache.TreeStructure;

namespace ODataWithSprache.Visitors;

public class TreeNodeBSqlFilterVisitor: TreeNodeVisitorBase<string>
{
    private const string _AssignOperator = "->>";
    
    /// <inheritdoc/>
    protected override string VisitBinaryExpressionNode(
        BinaryExpressionNode?  expression)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }
        
        
        if (expression.RightChild.GetType() == typeof(ExpressionNode))
        {
            return string.Join(
                " ",
                $"{VisitExpressionNode((ExpressionNode)expression.LeftChild)}",
                $"{expression.BinaryType.ToSqlOperator()}",
                $"{VisitExpressionNode((ExpressionNode)expression.RightChild)}");
        }

        return string.Join(
            " ",
            $"{VisitExpressionNode((ExpressionNode)expression.LeftChild)}",
            $"{expression.BinaryType.ToSqlOperator()}",
            $"{VisitBinaryExpressionNode((BinaryExpressionNode)expression.RightChild)}");
    }

    /// <inheritdoc/>
    protected override string VisitExpressionNode(ExpressionNode expression)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }
        
        return string.Join(
            " ",
            $"'{expression.LeftSideExpression}'",
            $"{expression.Operator.ToSqlOperator()}",
            $"{ParserHelper.ParserRightHandSide(expression.RightSideExpression)}");
    }

    /// <inheritdoc/>
    public override string Visit(RootNode? root, params string [] optionalParameter)
    {
        string variable = optionalParameter[0];

        if (string.IsNullOrWhiteSpace(variable))
        {
            throw new ArgumentException("No variable was defined for the where clause.");
        }
        
        if (root == null)
        {
            throw new ArgumentNullException(nameof(root));
        }
        
        if (root.LeftChild.GetType() == typeof(ExpressionNode))
        {
            return string.Join(
                " ",
                $"{root._operatorType.ToSqlOperator()}",
                $"{variable} {_AssignOperator}",
                $"{VisitExpressionNode((ExpressionNode)root.LeftChild)}");
        }

        return string.Join(
            " ",
            $"{root._operatorType.ToSqlOperator()}",
            $"{variable} {_AssignOperator}",
            $"{VisitBinaryExpressionNode((BinaryExpressionNode)root.LeftChild)}");
    }
}
