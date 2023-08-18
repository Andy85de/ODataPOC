using ODataWithSprache.Grammar;

namespace ODataWithSprache.TreeStructure
{
    public class RootNode : TreeNodeWithTwoChildren
    {
        private RootNode() : base(null)
        {
        }

        public ODataFilterOption _operatorType;

        public string OriginalString;


        public RootNode(ODataFilterOption @operator, string originalQuery) : base(null)
        {
            _operatorType = @operator;
            OriginalString = originalQuery;
        }
        
        private RootNode(string id) : base(null, id)
        {
        }

        public static RootNode Create(string id = null)
        {
            RootNode root = id == null ? new RootNode() : new RootNode(id);

            root.Root = root;

            return root;
        }

        public override TreeNode Clone(TreeNode newParent = null)
        {
            throw new NotImplementedException();
        }
    }
}
