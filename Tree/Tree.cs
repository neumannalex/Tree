using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeumannAlex.Tree
{
    public class Tree<T>
    {
        public TreeNode<T> TreeRoot { get; private set; }

        public int Count
        {
            get
            {
                return TreeRoot.Count() - 1;
            }
        }

        public Tree()
        {
            TreeRoot = new TreeNode<T>();
            TreeRoot.IsTreeRoot = true;
        }

        public ITreeNode<T> AddChild(ITreeNode<T> node)
        {
            return TreeRoot.AddChild(node);
        }

        public ITreeNode<T> AddChild(T value)
        {
            return TreeRoot.AddChild(value);
        }

        public ITreeNode<T> InsertBefore(ITreeNode<T> existingNode, ITreeNode<T> newNode)
        {
            if(ContainsNode(existingNode))
            {
                return existingNode.Parent.InsertBefore(existingNode, newNode);
            }
            else
            {
                throw new ArgumentOutOfRangeException("No node found that matches the parameter 'existingNode'.");
            }
        }

        public ITreeNode<T> InsertBefore(ITreeNode<T> existingNode, T value)
        {
            var newNode = new TreeNode<T>(value);

            return InsertBefore(existingNode, newNode);
        }

        public ITreeNode<T> InsertAfter(ITreeNode<T> existingNode, ITreeNode<T> newNode)
        {
            if (ContainsNode(existingNode))
            {
                return existingNode.Parent.InsertAfter(existingNode, newNode);
            }
            else
            {
                throw new ArgumentOutOfRangeException("No node found that matches the parameter 'existingNode'.");
            }
        }

        public ITreeNode<T> InsertAfter(ITreeNode<T> existingNode, T value)
        {
            var newNode = new TreeNode<T>(value);

            return InsertAfter(existingNode, newNode);
        }

        public ITreeNode<T> InsertBelow(ITreeNode<T> existingNode, ITreeNode<T> newNode)
        {
            if (ContainsNode(existingNode))
            {
                return existingNode.Parent.InsertBelow(existingNode, newNode);
            }
            else
            {
                throw new ArgumentOutOfRangeException("No node found that matches the parameter 'existingNode'.");
            }
        }

        public ITreeNode<T> InsertBelow(ITreeNode<T> existingNode, T value)
        {
            var newNode = new TreeNode<T>(value);

            return InsertBelow(existingNode, newNode);
        }

        public override string ToString()
        {
            var t = this.GetType();

            var treeType = "unknown";
            if (t.GenericTypeArguments.Count() > 0)
                treeType = t.GenericTypeArguments[0].Name;

            var numberOfDescandents = Count;

            if(numberOfDescandents == 1)
                return $"Tree of Type \"{treeType}\" with {Count} descendant node.";
            else
                return $"Tree of Type \"{treeType}\" with {Count} descendant nodes.";
        }

        public List<ITreeNode<T>> ToList(TreeTraverseOrder order = TreeTraverseOrder.DepthFirst)
        {
            var lst = TreeRoot.ToList(order);
            lst.Remove(TreeRoot);

            return lst;
        }

        public bool ContainsNode(ITreeNode<T> node)
        {
            return TreeRoot.Descendants().Contains(node);
        }

        public IEnumerable<ITreeNode<T>> Where(Func<ITreeNode<T>, bool> predicate)
        {
            return TreeRoot.Descendants().Where(predicate);
        }

        public ITreeNode<T> First(Func<ITreeNode<T>, bool> predicate)
        {
            return TreeRoot.Descendants().First(predicate);
        }

        public ITreeNode<T> FirstOrDefault(Func<ITreeNode<T>, bool> predicate)
        {
            return TreeRoot.Descendants().FirstOrDefault(predicate);
        }

        public ITreeNode<T> Last(Func<ITreeNode<T>, bool> predicate)
        {
            return TreeRoot.Descendants().Last(predicate);
        }

        public ITreeNode<T> LastOrDefault(Func<ITreeNode<T>, bool> predicate)
        {
            return TreeRoot.Descendants().LastOrDefault(predicate);
        }

        public IEnumerable<TResult> Select<TResult>(Func<ITreeNode<T>, TResult> selector)
        {
            return TreeRoot.Descendants().Select(selector);
        }

        public ITreeNode<T> NodeAtPath(List<int> path)
        {
            return Where(x => x.Path.SequenceEqual(path)).FirstOrDefault();
        }

        public ITreeNode<T> NodeAtPath(params int[] indices)
        {
            return NodeAtPath(indices.ToList());
        }
    }
}
