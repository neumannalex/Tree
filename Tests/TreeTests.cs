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

        public TreeNode<string> Node1 { get; set; }
        public TreeNode<string> Node11 { get; set; }

        public TreeNode<string> Node2 { get; set; }
        public TreeNode<string> Node21 { get; set; }
        public TreeNode<string> Node211 { get; set; }
        public TreeNode<string> Node22 { get; set; }

        public TreeNode<string> Node3 { get; set; }

        public int NumberOfNodes
        {
            get
            {
                var props = typeof(TestTreeData).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                var nodes = props.Where(x => x.PropertyType == typeof(TreeNode<string>));

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
        public void EmptyTree()
        {
            var tree = new Tree<string>();

            tree.Count.Should().Be(0);
            tree.IsRoot.Should().BeTrue();
            tree.Depth.Should().Be(0);
            tree.Path.Should().HaveCount(0);
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
        public void FilterTreeOneResult()
        {
            var result = _simpleTreeData.Tree.Where(x => x.Value == "node21");

            result.Should().HaveCount(1);
            result.ElementAt(0).Should().BeEquivalentTo(_simpleTreeData.Node21);
        }

        [Fact]
        public void FilterTreeMultipleResults()
        {
            var result = _simpleTreeData.Tree.Where(x => x.Depth == 2);

            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(new[] { _simpleTreeData.Node11, _simpleTreeData.Node21, _simpleTreeData.Node22 });
        }

        [Fact]
        public void FilterTreeNoResult()
        {
            var result = _simpleTreeData.Tree.Where(x => x.Value == "text");

            result.Should().HaveCount(0);
        }

        [Fact]
        public void LinqToTree()
        {
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
    }
}
