using ODataWithSprache.TreeStructure;

namespace ODataWithSprache.Extensions;

public static class TreeNodeExtensions
{
    public static ExpressionNode ToExpressionNode(this TreeNode node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return (ExpressionNode)node;
    }
    
    public static BinaryExpressionNode ToBinaryExpressionNode(this TreeNode node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return (BinaryExpressionNode)node;
    }
    
    public static RootNode ToRootNode(this TreeNode node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return (RootNode)node;
    }
}
