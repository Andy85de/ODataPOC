namespace ODataWithSprache.TreeStructure
{
    public abstract class TreeNode
    {
        public string Id { get; protected set; }
        public TreeNode Root { get; protected set; }
        public TreeNode Parent { get; protected set; }
        
        public string Type => GetType().Name;
        
        protected TreeNode(TreeNode parent)
            : this(parent, Guid.NewGuid().ToString("D"))
        {
        }

        protected TreeNode(TreeNode parent, string id)
        {
            Parent = parent;
            Root = parent?.Root;
            Id = id;
        }

        // Useful for the treenode builder
        internal void ChangeParent(TreeNode newParent)
        {
            Parent = newParent;
            Root = newParent?.Root;
        }

        public abstract TreeNode Clone(TreeNode newParent = null);

        /// <inheritdoc />
        public override string ToString()
        {
            return Type;
        }
    }
}
