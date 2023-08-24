using ODataWithSprache.Extensions;
using ODataWithSprache.Helpers;
using ODataWithSprache.TreeStructure;

namespace ODataWithSprache.Visitors;

public class TreeNodeVisitorString : TreeNodeVisitorBase<string>
{
    protected override string VisitBinaryExpressionNode(
        BinaryExpressionNode expression,
        params object[] optionalParameters)
    {
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

    protected override string VisitExpressionNode(ExpressionNode expression, params object[] optionalParameters)
    {
        return string.Join(
            " ",
            $"{expression.LeftSideExpression}",
            $"{expression.Operator.ToSqlOperator()}",
            $"{ParserHelper.ParserRightHandSide(expression.RightSideExpression)}");
    }

    public override string Visit(RootNode root)
    {
        if (root.LeftChild.GetType() == typeof(ExpressionNode))
        {
            return string.Join(
                " ",
                $"{root._operatorType.ToSqlOperator()}",
                $"{VisitExpressionNode((ExpressionNode)root.LeftChild)}");
        }

        return string.Join(
            " ",
            $"{root._operatorType.ToSqlOperator()}",
            $"{VisitBinaryExpressionNode((BinaryExpressionNode)root.LeftChild)}");
    }
}
