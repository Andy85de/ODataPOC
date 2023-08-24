using ODataWithSprache.Grammar;

namespace ODataWithSprache.TreeStructure;

public class RootNode : TreeNodeWithTwoChildren
{
    private RootNode(string originalString, TreeNode? parent) : base(parent)
    {
        _originalString = originalString;
    }

    public ODataFilterOption _operatorType;

    public string _originalString;

    public RootNode(
        ODataFilterOption @operator,
        string originalQuery,
        TreeNode? parent = null) : base(parent)
    {
        _operatorType = @operator;
        _originalString = originalQuery;
    }

    private RootNode(string id, string originalString) : base(null, id)
    {
        _originalString = originalString;
    }

    public override TreeNode Clone(TreeNode newParent = null)
    {
        throw new NotImplementedException();
    }
}