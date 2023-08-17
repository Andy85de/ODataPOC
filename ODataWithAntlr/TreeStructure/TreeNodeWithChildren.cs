using System.Reflection;

namespace ODataWithSprache.TreeStructure
{
    public abstract class TreeNodeWithChildren : TreeNode
    {
        public IList<TreeNode> Children { get; set; }

        protected TreeNodeWithChildren(TreeNode parent) : base(parent)
        {
        }

        protected TreeNodeWithChildren(TreeNode parent, string id)
            : base(parent, id)
        {
        }
        
        public virtual void RemoveChildren()
        {
            if (Children == null)
            {
                return;
            }

            foreach (TreeNode child in Children)
            {
                if (!(child is TreeNodeWithChildren childWihChildren))
                {
                    continue;
                }

                childWihChildren.RemoveChildren();
            }

            Children.Clear();
        }

        public virtual void RemoveChild(TreeNode child)
        {
            if (child == null
                || Children == null
                || !Children.Contains(child))
            {
                return;
            }

            if (child is TreeNodeWithChildren childWihChildren)
            {
                childWihChildren.RemoveChildren();
            }

            Children.Remove(child);
        }
        
        protected internal virtual TreeNode FindNodeById(string id)
        {
            if (Id.Equals(id, StringComparison.OrdinalIgnoreCase))
            {
                return this;
            }

            if (Children == null)
            {
                return null;
            }

            foreach (TreeNode child in Children)
            {
                if (child.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                {
                    return child;
                }

                if (!(child is TreeNodeWithChildren childWithChildren))
                {
                    continue;
                }

                TreeNode foundNode = childWithChildren.FindNodeById(id);

                if (foundNode != null)
                {
                    return foundNode;
                }
            }

            return null;
        }
    }
}
