using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeumannAlex.Tree
{
    public class TreeNode<T> : ITreeNode<T>
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

        #region Properties
        public ITreeNode<T> Root
        {
            get
            {
                var node = this as ITreeNode<T>;

                while (!node.IsRoot)
                {
                    node = node.Parent;
                }

                return node;
            }
        }

        public ITreeNode<T> Parent { get; set; }

        public List<ITreeNode<T>> Children { get; set; } = new List<ITreeNode<T>>();

        public T Value { get; set; }

        public virtual bool IsRoot => Parent == null;

        public bool IsTreeRoot { get; internal set; }

        public virtual bool HasChildren => Children.Count > 0;

        public virtual bool HasValue => Value != null;

        public virtual int Depth
        {
            get
            {
                var depth = 0;

                var node = this as ITreeNode<T>;
                while(true)
                {
                    depth++;

                    if (node.IsRoot)
                    {
                        if (node.IsTreeRoot)
                            depth--;

                        break;
                    }
                        
                    node = node.Parent;
                }

                return depth;
            }
        }

        public virtual List<int> Path
        {
            get
            {
                return GetPath(this);

                static List<int> GetPath(ITreeNode<T> node)
                {
                    if (node.IsRoot)
                    {
                        if (node.IsTreeRoot)
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

        public virtual string PathString
        {
            get
            {
                return string.Join('.', Path);
            }
        }

        public virtual int Count
        {
            get
            {
                return Descendants().Count;
            }
        }
        #endregion

        #region Methods
        public ITreeNode<T> AddChild(ITreeNode<T> child)
        {
            child.Parent = this;

            Children.Add(child);

            return child;
        }

        public ITreeNode<T> AddChild(T value)
        {
            var child = new TreeNode<T>(value)
            {
                Parent = this
            };

            Children.Add(child);

            return child;
        }

        public ITreeNode<T> InsertBefore(ITreeNode<T> existingNode, ITreeNode<T> newNode)
        {
            if(Children.Contains(existingNode))
            {
                var nodeIndex = Children.IndexOf(existingNode);

                newNode.Parent = this;

                Children.Insert(nodeIndex, newNode);

                return newNode;
            }
            else
            {
                throw new ArgumentOutOfRangeException("No child found that matches the parameter 'existingNode'.");
            }
        }

        public ITreeNode<T> InsertBefore(ITreeNode<T> existingNode, T value)
        {
            var newNode = new TreeNode<T>(value);

            return InsertBefore(existingNode, newNode);
        }

        public ITreeNode<T> InsertAfter(ITreeNode<T> existingNode, ITreeNode<T> newNode)
        {
            if (Children.Contains(existingNode))
            {
                var nodeIndex = Children.IndexOf(existingNode) + 1;

                newNode.Parent = this;

                Children.Insert(nodeIndex, newNode);

                return newNode;
            }
            else
            {
                throw new ArgumentOutOfRangeException("No child found that matches the parameter 'existingNode'.");
            }
        }

        public ITreeNode<T> InsertAfter(ITreeNode<T> existingNode, T value)
        {
            var newNode = new TreeNode<T>(value);

            return InsertAfter(existingNode, newNode);
        }

        public ITreeNode<T> InsertBelow(ITreeNode<T> existingNode, ITreeNode<T> newNode)
        {
            if (Children.Contains(existingNode))
            {
                return existingNode.AddChild(newNode);
            }
            else
            {
                throw new ArgumentOutOfRangeException("No child found that matches the parameter 'existingNode'.");
            }
        }

        public ITreeNode<T> InsertBelow(ITreeNode<T> existingNode, T value)
        {
            var newNode = new TreeNode<T>(value);

            return InsertBelow(existingNode, newNode);
        }

        public bool Remove()
        {
            if (Parent == null)
                return false;

            return Parent.Children.Remove(this);
        }

        public bool Remove(ITreeNode<T> existingNode)
        {
            if (Children.Contains(existingNode))
            {
                return Children.Remove(existingNode);
            }
            else
            {
                throw new ArgumentOutOfRangeException("No child found that matches the parameter 'existingNode'.");
            }
        }

        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= Children.Count)
                throw new ArgumentOutOfRangeException($"No child found at index '{index}'.");

            Children.RemoveAt(index);

            return true;
        }

        public List<ITreeNode<T>> Ancestors()
        {
            return GetAncestors(this);

            static List<ITreeNode<T>> GetAncestors(ITreeNode<T> node)
            {
                var ancestors = new List<ITreeNode<T>>();

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

        public List<ITreeNode<T>> Descendants()
        {
            return GetDescendants(this);

            static List<ITreeNode<T>> GetDescendants(ITreeNode<T> node)
            {
                var descendants = new List<ITreeNode<T>>();


                foreach (ITreeNode<T> child in node.Children)
                {
                    descendants.Add(child);

                    var childNodes = GetDescendants(child);

                    if (childNodes.Count > 0)
                        descendants.AddRange(childNodes);
                }

                return descendants;
            }
        }

        public List<ITreeNode<T>> Siblings()
        {
            if(Parent != null)
            {
                var siblings = Parent.Children;
                
                siblings.Remove(this);
                
                return siblings;
            }
            else
            {
                return new List<ITreeNode<T>>();
            }
        }

        public List<ITreeNode<T>> Predecessors()
        {
            if (Parent != null)
            {
                var myIndex = Parent.Children.IndexOf(this);

                return Parent.Children.GetRange(0, myIndex);
            }
            else
            {
                return new List<ITreeNode<T>>();
            }
        }

        public List<ITreeNode<T>> Successors()
        {
            if (Parent != null)
            {
                var myIndex = Parent.Children.IndexOf(this);

                return Parent.Children.GetRange(myIndex + 1, Parent.Children.Count - myIndex - 1);
            }
            else
            {
                return new List<ITreeNode<T>>();
            }
        }

        public override string ToString()
        {
            var value = HasValue ? $"\"{Value}\"" : "\"<empty>\"";

            return $"[{PathString}] {value} Children={Children.Count} Depth={Depth} IsRoot={IsRoot}";
        }

        public List<ITreeNode<T>> ToList(TreeTraverseOrder order = TreeTraverseOrder.DepthFirst)
        {
            if (order == TreeTraverseOrder.DepthFirst)
                return GetNodesDepthFirst(this);
            else
                return GetNodesBreadthFirst(this);
        }

        private List<ITreeNode<T>> GetNodesDepthFirst(ITreeNode<T> node)
        {
            var nodes = new List<ITreeNode<T>>();

            if (!(node is Tree<T>))
                nodes.Add(node);

            foreach (ITreeNode<T> child in node.Children)
            {
                var childNodes = GetNodesDepthFirst(child);
                nodes.AddRange(childNodes);
            }

            return nodes;
        }

        private List<ITreeNode<T>> GetNodesBreadthFirst(ITreeNode<T> root)
        {
            List<ITreeNode<T>> sortedNodes = new List<ITreeNode<T>>();

            Queue<ITreeNode<T>> queue = new Queue<ITreeNode<T>>();
            HashSet<ITreeNode<T>> set = new HashSet<ITreeNode<T>>();

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
        public IEnumerator<ITreeNode<T>> GetEnumerator()
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
