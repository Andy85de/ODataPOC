using ODataWithSprache.Grammar;

namespace ODataWithSprache.TreeStructure
{
    public class BinaryExpression : TreeNodeWithTwoChildren
    {
        public ExpressionCombinator BinaryType { get; set; }

        public BinaryExpression(TreeNode parent) : base(parent)
        {
        }

        public BinaryExpression(TreeNode left, TreeNode right, ExpressionCombinator op) : base(left,right)
        {
            BinaryType = op;
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
