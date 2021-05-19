using System;
using System.Collections.Generic;

namespace NeumannAlex.Tree
{
    public interface ITreeNode<T> : IEnumerable<ITreeNode<T>>
    {
        ITreeNode<T> Root { get; }

        ITreeNode<T> Parent { get; set; }

        List<ITreeNode<T>> Children { get; set; }

        T Value { get; set; }

        bool IsRoot { get; }

        bool IsTreeRoot { get; }

        bool HasChildren { get; }

        bool HasValue { get; }

        int Depth { get; }

        List<int> Path { get; }

        string PathString { get; }

        int Count { get; }


        ITreeNode<T> AddChild(ITreeNode<T> child);
        
        ITreeNode<T> AddChild(T value);

        ITreeNode<T> InsertBefore(ITreeNode<T> existingNode, ITreeNode<T> newNode);

        ITreeNode<T> InsertBefore(ITreeNode<T> existingNode, T value);

        ITreeNode<T> InsertAfter(ITreeNode<T> existingNode, ITreeNode<T> newNode);

        ITreeNode<T> InsertAfter(ITreeNode<T> existingNode, T value);

        ITreeNode<T> InsertBelow(ITreeNode<T> existingNode, ITreeNode<T> newNode);

        ITreeNode<T> InsertBelow(ITreeNode<T> existingNode, T value);

        bool Remove();

        bool Remove(ITreeNode<T> existingNode);

        bool RemoveAt(int index);

        List<ITreeNode<T>> Ancestors();
        
        List<ITreeNode<T>> Descendants();

        List<ITreeNode<T>> Siblings();

        List<ITreeNode<T>> Predecessors();

        List<ITreeNode<T>> Successors();

        string ToString();

        string ToText(string indent, Func<ITreeNode<T>, string> nodeFormat);

        List<ITreeNode<T>> ToList(TreeTraverseOrder order = TreeTraverseOrder.DepthFirst);
        
    }
}