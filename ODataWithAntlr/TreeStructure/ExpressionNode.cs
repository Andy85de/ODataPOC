using Antlr4.Runtime.Atn;
using ODataWithSprache.Grammar;

namespace ODataWithSprache.TreeStructure
{
    public class ExpressionNode : TreeNode
    {
        public string LeftSideExpression { get; set; }
        public OperatorType Operator { get; set; }
        
        public string RightSideExpression { get; set; }
        public ValueType ValueType { get; set; }

        public ExpressionNode(TreeNode parent)
            : base(parent)
        {
        }

        public ExpressionNode(string leftSideExpression, string rightSideExpression, OperatorType @operator) : base(
            null)
        {
            LeftSideExpression = leftSideExpression;
            RightSideExpression = rightSideExpression;
            Operator = @operator;
        }
        
        protected ExpressionNode(TreeNode parent, string id)
            : base(parent, id)
        {
        }

        public override TreeNode Clone(TreeNode newParent = null)
        {
            return new ExpressionNode(newParent, Id)
            {
                ValueType = ValueType,
                Operator = Operator,
                LeftSideExpression = LeftSideExpression,
                RightSideExpression = RightSideExpression
            };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Concat(LeftSideExpression, Operator, RightSideExpression);
        }
    }
}
