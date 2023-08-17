namespace ODataWithSprache.TreeStructure;

public class TreeNodeWithTwoChildren: TreeNode
{
    public TreeNode LeftChild { set; get; }

    public TreeNode RightChild { get; set; }
    
    
    public TreeNodeWithTwoChildren(TreeNode parent) : base(parent)
    {
    }

    public TreeNodeWithTwoChildren(TreeNode left, TreeNode right):base(null)
    {
        LeftChild = left;
        RightChild = right;
    }
    
    public TreeNodeWithTwoChildren(TreeNode parent, string id) : base(parent, id)
    {
    }

    public override TreeNode Clone(TreeNode newParent = null)
    {
        throw new NotImplementedException();
    }
}
