using ODataWithSprache.Grammar;

namespace ODataWithSprache.TreeStructure
{
    public class BinaryExpressionNode : TreeNodeWithTwoChildren
    {
        public ExpressionCombinator BinaryType { get; set; }

        public BinaryExpressionNode(TreeNode parent) : base(parent)
        {
        }

        public BinaryExpressionNode(TreeNode left, TreeNode right, ExpressionCombinator op) : base(left,right)
        {
            BinaryType = op;
        }
        
        protected BinaryExpressionNode(TreeNode parent, string id)
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
