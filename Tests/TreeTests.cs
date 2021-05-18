using FluentAssertions;
using NeumannAlex.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    class TestTreeData
    {
        public Tree<string> Tree { get; set; }

        public ITreeNode<string> Node1 { get; set; }
        public ITreeNode<string> Node11 { get; set; }

        public ITreeNode<string> Node2 { get; set; }
        public ITreeNode<string> Node21 { get; set; }
        public ITreeNode<string> Node211 { get; set; }
        public ITreeNode<string> Node22 { get; set; }

        public ITreeNode<string> Node3 { get; set; }

        public int NumberOfNodes
        {
            get
            {
                var props = typeof(TestTreeData).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                var nodes = props.Where(x => x.PropertyType == typeof(ITreeNode<string>));

                return nodes.Count();
            }
        }

        public TestTreeData()
        {
            Tree = new Tree<string>();

            Node1 = Tree.AddChild("node1");
            Node2 = Tree.AddChild("node2");
            Node3 = Tree.AddChild("node3");

            Node11 = Node1.AddChild("node11");

            Node21 = Node2.AddChild("node21");
            Node22 = Node2.AddChild("node22");

            Node211 = Node21.AddChild("node211");
        }
    }

    public class TreeTests
    {
        private readonly TestTreeData _simpleTreeData;

        public TreeTests()
        {
            _simpleTreeData = new TestTreeData();
        }

        [Fact]
        public void SmallestTree()
        {
            var tree = new Tree<string>();

            tree.Count.Should().Be(0);
            tree.TreeRoot.Should().NotBeNull();
        }

        [Fact]
        public void SimpleTree()
        {
            _simpleTreeData.Tree.Count.Should().Be(_simpleTreeData.NumberOfNodes);

            _simpleTreeData.Node1.Depth.Should().Be(1);
            _simpleTreeData.Node211.Depth.Should().Be(3);
            _simpleTreeData.Node211.Path.Should().Equal(new List<int> { 2, 1, 1 });
        }

        [Fact]
        public void SimpleTreeToList()
        {
            var lst = _simpleTreeData.Tree.ToList();
            lst.Should().HaveCount(_simpleTreeData.NumberOfNodes);
        }

        [Fact]
        public void LinqToTree()
        {
            var filterOneResult = _simpleTreeData.Tree.Where(x => x.Value == "node21");
            filterOneResult.Should().HaveCount(1);
            filterOneResult.ElementAt(0).Should().BeEquivalentTo(_simpleTreeData.Node21);

            var filterMultipleResult = _simpleTreeData.Tree.Where(x => x.Depth == 2);
            filterMultipleResult.Should().HaveCount(3);
            filterMultipleResult.Should().BeEquivalentTo(new[] { _simpleTreeData.Node11, _simpleTreeData.Node21, _simpleTreeData.Node22 });


            var filterNoResult = _simpleTreeData.Tree.Where(x => x.Value == "text");
            filterNoResult.Should().HaveCount(0);

            var firstResult = _simpleTreeData.Tree.First(x => x.Depth == 2);
            firstResult.Should().BeEquivalentTo(_simpleTreeData.Node11);

            var firstResultNull = _simpleTreeData.Tree.FirstOrDefault(x => x.Depth == 4);
            firstResultNull.Should().BeNull();

            var lastResult = _simpleTreeData.Tree.Last(x => x.Depth == 2);
            lastResult.Should().BeEquivalentTo(_simpleTreeData.Node22);

            var selectResult = _simpleTreeData.Tree.Select(x => x.Value);
            selectResult.Should().HaveCount(_simpleTreeData.NumberOfNodes);

            var filterResult = _simpleTreeData.Tree.Where(x => x.Depth == 2);
            filterResult.Should().HaveCount(3);
            filterResult.Should().ContainInOrder(new[] { _simpleTreeData.Node11, _simpleTreeData.Node21, _simpleTreeData.Node22 });

            var orderResult = _simpleTreeData.Tree.Where(x => x.Depth == 2).OrderByDescending(x => x.Value);
            orderResult.Should().HaveCount(3);
            orderResult.Should().ContainInOrder(new[] { _simpleTreeData.Node22, _simpleTreeData.Node21, _simpleTreeData.Node11 });
        }
    
        [Fact]
        public void TreeToString()
        {
            var text = _simpleTreeData.Tree.ToString();

            text.Should().Contain("\"String\"")
                .And.Contain(_simpleTreeData.NumberOfNodes.ToString());
        }
        
        [Fact]
        public void ElementAtSuccess()
        {
            var result1 = _simpleTreeData.Tree.NodeAtPath(new List<int> { 2, 2 });

            result1.Should().BeEquivalentTo(_simpleTreeData.Node22);

            var result2 = _simpleTreeData.Tree.NodeAtPath(2, 2);

            result2.Should().BeEquivalentTo(_simpleTreeData.Node22);
        }

        [Fact]
        public void ElementAtFail()
        {
            var result = _simpleTreeData.Tree.NodeAtPath(new List<int> { 9, 9 });

            result.Should().BeNull();
        }

        [Fact]
        public void ContainsNodeSuccess()
        {
            

            var containsTrue = _simpleTreeData.Tree.ContainsNode(_simpleTreeData.Node211);

            containsTrue.Should().BeTrue();

            var copy = new TreeNode<string>("node211");
            var containsFalse = _simpleTreeData.Tree.ContainsNode(copy);

            containsFalse.Should().BeFalse();
        }

        [Fact]
        public void ContainsNodeFail()
        {
            var invalid = new TreeNode<string>("invalid");

            var contains = _simpleTreeData.Tree.ContainsNode(invalid);

            contains.Should().BeFalse();
        }

        [Fact]
        public void InsertBefore()
        {
            
            _simpleTreeData.Tree.TreeRoot.Children.Should().HaveCount(3);
            _simpleTreeData.Tree.TreeRoot.Children[0].Should().BeEquivalentTo(_simpleTreeData.Node1);
            _simpleTreeData.Tree.TreeRoot.Children[1].Should().BeEquivalentTo(_simpleTreeData.Node2);
            _simpleTreeData.Tree.TreeRoot.Children[2].Should().BeEquivalentTo(_simpleTreeData.Node3);

            var insertedNode1 = _simpleTreeData.Tree.InsertBefore(_simpleTreeData.Node3, new TreeNode<string>("insertedNode1"));

            _simpleTreeData.Tree.TreeRoot.Children.Should().HaveCount(4);
            _simpleTreeData.Tree.TreeRoot.Children[0].Should().BeEquivalentTo(_simpleTreeData.Node1);
            _simpleTreeData.Tree.TreeRoot.Children[1].Should().BeEquivalentTo(_simpleTreeData.Node2);
            _simpleTreeData.Tree.TreeRoot.Children[2].Should().BeEquivalentTo(insertedNode1);
            _simpleTreeData.Tree.TreeRoot.Children[3].Should().BeEquivalentTo(_simpleTreeData.Node3);
        }

        [Fact]
        public void InsertAfter()
        {

            _simpleTreeData.Tree.TreeRoot.Children.Should().HaveCount(3);
            _simpleTreeData.Tree.TreeRoot.Children[0].Should().BeEquivalentTo(_simpleTreeData.Node1);
            _simpleTreeData.Tree.TreeRoot.Children[1].Should().BeEquivalentTo(_simpleTreeData.Node2);
            _simpleTreeData.Tree.TreeRoot.Children[2].Should().BeEquivalentTo(_simpleTreeData.Node3);

            var insertedNode1 = _simpleTreeData.Tree.InsertAfter(_simpleTreeData.Node2, new TreeNode<string>("insertedNode1"));

            _simpleTreeData.Tree.TreeRoot.Children.Should().HaveCount(4);
            _simpleTreeData.Tree.TreeRoot.Children[0].Should().BeEquivalentTo(_simpleTreeData.Node1);
            _simpleTreeData.Tree.TreeRoot.Children[1].Should().BeEquivalentTo(_simpleTreeData.Node2);
            _simpleTreeData.Tree.TreeRoot.Children[2].Should().BeEquivalentTo(insertedNode1);
            _simpleTreeData.Tree.TreeRoot.Children[3].Should().BeEquivalentTo(_simpleTreeData.Node3);
        }

        [Fact]
        public void InsertBelow()
        {

            _simpleTreeData.Tree.TreeRoot.Children.Should().HaveCount(3);
            _simpleTreeData.Tree.TreeRoot.Children[0].Should().BeEquivalentTo(_simpleTreeData.Node1);
            _simpleTreeData.Tree.TreeRoot.Children[1].Should().BeEquivalentTo(_simpleTreeData.Node2);
            _simpleTreeData.Tree.TreeRoot.Children[2].Should().BeEquivalentTo(_simpleTreeData.Node3);

            var initialChildCount = _simpleTreeData.Node2.Children.Count;

            var insertedNode1 = _simpleTreeData.Tree.InsertBelow(_simpleTreeData.Node2, new TreeNode<string>("insertedNode1"));

            _simpleTreeData.Tree.TreeRoot.Children.Should().HaveCount(3);
            _simpleTreeData.Tree.TreeRoot.Children[0].Should().BeEquivalentTo(_simpleTreeData.Node1);
            _simpleTreeData.Tree.TreeRoot.Children[1].Should().BeEquivalentTo(_simpleTreeData.Node2);
            _simpleTreeData.Tree.TreeRoot.Children[2].Should().BeEquivalentTo(_simpleTreeData.Node3);

            _simpleTreeData.Node2.Children.Count.Should().Be(initialChildCount + 1);
            _simpleTreeData.Node2.Children[^1].Should().BeEquivalentTo(insertedNode1);
        }
    }
}
