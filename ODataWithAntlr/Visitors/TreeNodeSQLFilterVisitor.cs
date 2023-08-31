﻿using ODataWithSprache.Extensions;
using ODataWithSprache.Helpers;
using ODataWithSprache.TreeStructure;

namespace ODataWithSprache.Visitors;

/// <summary>
///     An implementation of the <see cref="TreeNodeVisitorBase{TResult}" /> visitor class.
/// </summary>
public class TreeNodeSQLFilterVisitor : TreeNodeVisitorBase<string>
{
    private readonly ILogger<TreeNodeSQLFilterVisitor> _Logger;

    public TreeNodeSQLFilterVisitor(ILogger<TreeNodeSQLFilterVisitor> logger)
    {
        _Logger = logger;
    }

    public TreeNodeSQLFilterVisitor()
    {
    }
    

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
            $"{expression.LeftSideExpression}",
            $"{expression.Operator.ToSqlOperator()}",
            $"{ParserHelper.ParserRightHandSide(expression.RightSideExpression)}");
    }

    /// <inheritdoc/>
    public override string Visit(RootNode root, params string[] optionalParameter )
    {
        if (root == null)
        {
            throw new ArgumentNullException(nameof(root));
        }
        
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