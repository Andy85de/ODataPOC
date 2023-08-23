using ODataWithSprache.Grammar;

namespace ODataWithSprache.TreeStructure;

public class ExpressionNode : TreeNode
{
    public string LeftSideExpression { get; set; }
    public OperatorType Operator { get; set; }
    public string RightSideExpression { get; set; }

    public ExpressionNode(TreeNode parent, OperatorType operatorType,string leftSideExpression, string rightSideExpression)
        : base(parent)
    {
        LeftSideExpression = leftSideExpression;
        RightSideExpression = rightSideExpression;
        Operator = operatorType;
    }

    public ExpressionNode(
        string leftSideExpression,
        string rightSideExpression,
        OperatorType @operator) : base(null)
    {
        LeftSideExpression = leftSideExpression;
        RightSideExpression = rightSideExpression;
        Operator = @operator;
    }

    protected ExpressionNode(
        TreeNode parent,
        string id,
        string leftSideExpression,
        string rightSideExpression)
        : base(parent, id)
    {
        LeftSideExpression = leftSideExpression;
        RightSideExpression = rightSideExpression;
    }

    public override TreeNode Clone(TreeNode newParent = null)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return string.Concat(LeftSideExpression, Operator, RightSideExpression);
    }
}