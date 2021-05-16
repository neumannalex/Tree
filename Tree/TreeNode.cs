using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NeumannAlex.Tree
{
    public class TreeNode<T> : IEnumerable<TreeNode<T>>
    {
        #region Ctor
        public TreeNode()
        {
            Value = default;
        }

        public TreeNode(T value)
        {
            Value = value;
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        public TreeNode<T> Root
        {
            get
            {
                var node = this;

                while (!node.IsRoot)
                {
                    node = node.Parent;
                }

                return node;
            }
        }

        public TreeNode<T> Parent { get; private set; }

        public List<TreeNode<T>> Children { get; private set; } = new List<TreeNode<T>>();

        public T Value { get; set; }

        public bool IsRoot => Parent == null;

        public bool HasChildren => Children.Count > 0;

        public bool HasValue => Value != null;  // Geht das auch mit int etc?

        public int Depth
        {
            get
            {
                var depth = 0;

                var node = this;
                while (!node.IsRoot)
                {
                    node = node.Parent;
                    depth++;
                }

                return depth;
            }
        }

        public List<int> Path
        {
            get
            {
                return GetPath(this);

                static List<int> GetPath(TreeNode<T> node)
                {
                    if (node.IsRoot)
                    {
                        if (node is Tree<T>)
                            return new List<int>();
                        else
                            return new List<int> { 1 };
                    }
                    else
                    {
                        var path = GetPath(node.Parent);

                        var nodeIndex = node.Parent.Children.IndexOf(node) + 1;

                        path.Add(nodeIndex);

                        return path;
                    }
                }
            }
        }

        public int Count
        {
            get
            {
                return Descendants().Count;
            }
        }
        #endregion

        #region Methods
        public TreeNode<T> AddChild(TreeNode<T> child)
        {
            child.Parent = this;

            Children.Add(child);

            return child;
        }

        public TreeNode<T> AddChild(T value)
        {
            var child = new TreeNode<T>(value)
            {
                Parent = this
            };

            Children.Add(child);

            return child;
        }

        public List<TreeNode<T>> Ancestors()
        {
            return GetAncestors(this);

            static List<TreeNode<T>> GetAncestors(TreeNode<T> node)
            {
                var ancestors = new List<TreeNode<T>>();

                if (node.Parent != null)
                {
                    ancestors.Add(node.Parent);

                    var nodes = GetAncestors(node.Parent);
                    if (nodes.Count > 0)
                        ancestors.AddRange(nodes);
                }

                return ancestors;
            }

        }

        public List<TreeNode<T>> Descendants()
        {
            return GetDescendants(this);

            static List<TreeNode<T>> GetDescendants(TreeNode<T> node)
            {
                var descendants = new List<TreeNode<T>>();

                //descendants.Add(node);

                foreach (var child in node.Children)
                {
                    descendants.Add(child);

                    var childNodes = GetDescendants(child);

                    if (childNodes.Count > 0)
                        descendants.AddRange(childNodes);
                }

                return descendants;
            }
        }

        public override string ToString()
        {
            var path = string.Join('.', Path);
            var value = HasValue ? $"\"{Value}\"" : "\"<empty>\"";

            return $"[{path}] {value} Children={Children.Count} Depth={Depth} IsRoot={IsRoot}";
        }

        public List<TreeNode<T>> ToList(TreeTraverseOrder order = TreeTraverseOrder.DepthFirst)
        {
            if (order == TreeTraverseOrder.DepthFirst)
                return GetNodesDepthFirst(this);
            else
                return GetNodesBreadthFirst(this);
        }

        private List<TreeNode<T>> GetNodesDepthFirst(TreeNode<T> node)
        {
            var nodes = new List<TreeNode<T>>();

            if(!(node is Tree<T>))
                nodes.Add(node);

            foreach (var child in node.Children)
            {
                var childNodes = GetNodesDepthFirst(child);
                nodes.AddRange(childNodes);
            }

            return nodes;
        }

        private List<TreeNode<T>> GetNodesBreadthFirst(TreeNode<T> root)
        {
            List<TreeNode<T>> sortedNodes = new List<TreeNode<T>>();

            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            HashSet<TreeNode<T>> set = new HashSet<TreeNode<T>>();

            queue.Enqueue(root);
            set.Add(root);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                sortedNodes.Add(node);

                foreach (var child in node.Children)
                {
                    if (!set.Contains(child))
                    {
                        queue.Enqueue(child);
                        set.Add(child);
                    }
                }
            }

            return sortedNodes;
        }

        #endregion

        #region IEnumerable<T>
        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            foreach (var node in ToList())
                yield return node;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
