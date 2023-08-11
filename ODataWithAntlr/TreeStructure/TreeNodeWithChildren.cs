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

        //public override TreeNode Clone(TreeNode newParent = null)
        //{
        //    var newObj = (TreeNodeWithChildren) Activator.CreateInstance(GetType(), newParent);
        //    newObj.Children = Children?.Select(c => c?.Clone(newObj)).ToList();

        //    return newObj;
        //}

        protected virtual TNode Clone<TNode>(TreeNode newParent = null) where TNode : TreeNodeWithChildren
        {
#if NETSTANDARD1_6
            var types = new[] {typeof(TreeNode), typeof(string)};
            var ctor = typeof(TNode).GetTypeInfo()
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .FirstOrDefault(c => types.All(t => c.GetParameters().Any(para => para.ParameterType == t)));

            if (ctor == null)
                throw new MissingMethodException($"Cannot find constructor for type {typeof(TNode).FullName} with two parameters, {nameof(TreeNode)} and string.");

            var newObj = (TNode)ctor.Invoke(new object[]{newParent, Id});
#else
            var newObj = (TNode)Activator.CreateInstance(
                typeof(TNode),
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new object[] { newParent, Id },
                null);
#endif
            newObj.Children = Children?.Select(c => c?.Clone(newObj)).ToList();

            return newObj;
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

        protected internal virtual TNode FindNode<TNode>(Func<TNode, bool> criteriaFunction) where TNode : TreeNode
        {
            if (typeof(TNode) == GetType() && criteriaFunction(this as TNode))
            {
                return this as TNode;
            }

            if (Children == null)
            {
                return null;
            }

            foreach (TreeNode child in Children)
            {
                if (child is TNode specialNode && criteriaFunction(specialNode))
                {
                    return specialNode;
                }

                if (!(child is TreeNodeWithChildren childWithChildren))
                {
                    continue;
                }

                TNode foundNode = childWithChildren.FindNode(criteriaFunction);

                if (foundNode != null)
                {
                    return foundNode;
                }
            }

            return null;
        }
    }
}
