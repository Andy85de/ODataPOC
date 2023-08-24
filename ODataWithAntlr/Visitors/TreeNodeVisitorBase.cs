using ODataWithSprache.TreeStructure;

namespace ODataWithSprache.Visitors;

public abstract class TreeNodeVisitorBase<TResult>
{
    protected abstract TResult VisitBinaryExpressionNode(
        BinaryExpressionNode expression,
        params object[] optionalParameters);

    protected abstract TResult VisitExpressionNode(ExpressionNode property, params object[] optionalParameters);

    public abstract TResult Visit(RootNode root);
}