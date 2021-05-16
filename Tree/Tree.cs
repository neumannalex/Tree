using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeumannAlex.Tree
{
    public class Tree<T> : TreeNode<T>
    {
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
    }
}
