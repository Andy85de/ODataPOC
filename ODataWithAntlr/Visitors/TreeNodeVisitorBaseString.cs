using ODataWithSprache.TreeStructure;

namespace ODataWithSprache.Visitors;

public abstract class TreeNodeVisitorBaseForSQL<TResult>
{
        /// <summary>
    ///     Describes how a <see cref="BinaryExpressionNode" /> node should be visited.
    /// </summary>
    /// <param name="expression">The <see cref="BinaryExpressionNode" /> that should be visited.</param>
    /// <returns>
    ///     A
    ///     <typeparam name="TResult" />
    ///     that will be created after visiting the <see cref="BinaryExpressionNode" />.
    /// </returns>
    protected abstract TResult VisitBinaryExpressionNode(
        BinaryExpressionNode expression, params object[] optionalParameter);

    /// <summary>
    ///     Describes how a <see cref="ExpressionNode" /> node should be visited.
    /// </summary>
    /// <param name="property">The <see cref="ExpressionNode" /> that should be visited.</param>
    /// <returns>
    ///     A
    ///     <typeparam name="TResult" />
    ///     that will be created after visiting the <see cref="ExpressionNode" />.
    /// </returns>
    protected abstract TResult VisitExpressionNode(ExpressionNode property, params object[] optionalParameter);


    protected virtual TResult VisitNode(TreeNode node, params object[] optionalParameter)
    {
        return node switch
        {
            ExpressionNode expressionNode => VisitExpressionNode(expressionNode, optionalParameter),
            BinaryExpressionNode binaryExpressionNode => VisitBinaryExpressionNode(
                binaryExpressionNode,
                optionalParameter),
            RootNode rootNode => VisitNode(rootNode.LeftChild, optionalParameter),
            _ => throw new ArgumentOutOfRangeException(nameof(node))
        };
    }
    
    /// <summary>
    ///     Describes how a <see cref="RootNode" /> node should be visited.
    /// </summary>
    /// <param name="root">The <see cref="RootNode" /> that should be visited.</param>
    /// <param name="optionalParameter">Optional parameter that are seperated though a comma.</param>
    /// <returns>
    ///     A
    ///     <typeparam name="TResult" />
    ///     that will be created after visiting the <see cref="RootNode" />.
    /// </returns>
    public abstract TResult Visit(RootNode root,  params object [] optionalParameter);
}
