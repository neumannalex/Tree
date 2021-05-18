using NeumannAlex.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    public enum MyTreeNodeType
    {
        Folder,
        Leaf
    }

    public class MyTypedTreeNode : TreeNode<Dictionary<string, string>>
    {
        public MyTypedTreeNode(Dictionary<string, string> value, MyTreeNodeType type = MyTreeNodeType.Leaf) : base(value)
        {
            Type = type;
        }

        public MyTypedTreeNode(MyTreeNodeType type = MyTreeNodeType.Leaf) : base()
        {
            Type = type;
        }

        public MyTreeNodeType Type { get; set; }        

        public override List<int> Path
        {
            get
            {
                return GetMyPath(this);

                static List<int> GetMyPath(MyTypedTreeNode node)
                {
                    if (node.IsRoot)
                    {
                        if (node is Tree<Dictionary<string, string>>)
                            return new List<int>();
                        else
                            return new List<int> { 1 };
                    }
                    else
                    {
                        var path = GetMyPath((MyTypedTreeNode)node.Parent);

                        var nodeIndex = node.Parent.Children.Where(n => (n as MyTypedTreeNode).Type == node.Type).ToList().IndexOf(node) + 1;

                        path.Add(nodeIndex);

                        return path;
                    }
                }
            }
        }

        public override string PathString
        {
            get
            {
                if (this.Type == MyTreeNodeType.Folder)
                    return string.Join('.', Path);
                else
                    return string.Join('.', Path.GetRange(0, Path.Count - 1)) + "-" + Path[^1];
            }
        }
    }

    public class MyGenericTreeNode<T> : TreeNode<T>
    {
        public MyGenericTreeNode(T value, MyTreeNodeType type = MyTreeNodeType.Leaf) : base(value)
        {
            Type = type;
        }

        public MyGenericTreeNode(MyTreeNodeType type = MyTreeNodeType.Leaf) : base()
        {
            Type = type;
        }

        public MyTreeNodeType Type { get; set; }

        public override List<int> Path
        {
            get
            {
                return GetMyPath(this);

                static List<int> GetMyPath(MyGenericTreeNode<T> node)
                {
                    if (node.IsRoot)
                    {
                        if (node is Tree<Dictionary<string, string>>)
                            return new List<int>();
                        else
                            return new List<int> { 1 };
                    }
                    else
                    {
                        var path = GetMyPath((MyGenericTreeNode<T>)node.Parent);

                        var nodeIndex = node.Parent.Children.Where(n => (n as MyGenericTreeNode<T>).Type == node.Type).ToList().IndexOf(node) + 1;

                        path.Add(nodeIndex);

                        return path;
                    }
                }
            }
        }

        public override string PathString
        {
            get
            {
                if (this.Type == MyTreeNodeType.Folder)
                    return string.Join('.', Path);
                else
                    return string.Join('.', Path.GetRange(0, Path.Count - 1)) + "-" + Path[^1];
            }
        }
    }
}
