namespace ODataWithSprache.TreeStructure
{
    public class BinaryExpression : TreeNodeWithTwoChildren
    {
        public BinaryType BinaryType { get; set; }

        public BinaryExpression(TreeNode parent) : base(parent)
        {
        }

        protected BinaryExpression(TreeNode parent, string id)
            : base(parent, id)
        {
        }
        
        /// <inheritdoc />
        public override string ToString()
        {
            return $"{BinaryType}";
        }
    }
}
