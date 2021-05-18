using FluentAssertions;
using NeumannAlex.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    class My
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class TreeNodeTests
    {
        [Fact]
        public void TestDefault()
        {
            var emptyNodeValueType = new TreeNode<int?>();
            emptyNodeValueType.HasValue.Should().BeFalse();

            var nonEmptyNodeValueType = new TreeNode<int?>(1);
            nonEmptyNodeValueType.HasValue.Should().BeTrue();

            var emptyNodeReferenceType = new TreeNode<string>();
            emptyNodeReferenceType.HasValue.Should().BeFalse();

            var nonEmptyNodeReferenceType = new TreeNode<string>("text");
            nonEmptyNodeReferenceType.HasValue.Should().BeTrue();

            var emptyNodeClassType = new TreeNode<My>();
            emptyNodeClassType.HasValue.Should().BeFalse();

            var nonEmptyNodeClassType = new TreeNode<My>(new My { Text = "text", Value = 42 });
            nonEmptyNodeClassType.HasValue.Should().BeTrue();
        }

        [Fact]
        public void EmptyNodeOfReferenceType()
        {
            var node0 = new TreeNode<string>();

            node0.HasChildren.Should().BeFalse();
            node0.HasValue.Should().BeFalse();
            node0.Depth.Should().Be(1);
            node0.Parent.Should().BeNull();
        }

        [Fact]
        public void EmptyNodeOfValueType()
        {
            var node0 = new TreeNode<int?>();

            node0.HasChildren.Should().BeFalse();
            node0.HasValue.Should().BeFalse();
            node0.Depth.Should().Be(1);
            node0.Parent.Should().BeNull();
        }

        [Fact]
        public void NonEmptyNode()
        {
            var node0 = new TreeNode<string>("node0");

            node0.HasChildren.Should().BeFalse();
            node0.HasValue.Should().BeTrue();
            node0.Depth.Should().Be(1);
            node0.Parent.Should().BeNull();
        }

        [Fact]
        public void NodeWithChildren()
        {
            var node0 = new TreeNode<string>("node0");

            var node10 = node0.AddChild("node10");
            var node20 = node0.AddChild("node20");

            var node11 = node10.AddChild("node11");

            node0.HasChildren.Should().BeTrue();
            node0.HasValue.Should().BeTrue();
            node0.Depth.Should().Be(1);
            node0.Parent.Should().BeNull();
            node0.Children.Count.Should().Be(2);

            node10.HasChildren.Should().BeTrue();
            node10.HasValue.Should().BeTrue();
            node10.Depth.Should().Be(2);
            node10.Parent.Should().BeEquivalentTo(node0);

            node20.HasChildren.Should().BeFalse();
            node20.HasValue.Should().BeTrue();
            node20.Depth.Should().Be(2);
            node20.Parent.Should().BeEquivalentTo(node0);

            node11.HasChildren.Should().BeFalse();
            node11.HasValue.Should().BeTrue();
            node11.Depth.Should().Be(3);
            node11.Parent.Should().BeEquivalentTo(node10);
            node11.Parent.Parent.Should().BeEquivalentTo(node0);
        }

        [Fact]
        public void GetAncestors()
        {
            var node0 = new TreeNode<string>("node0");

            var node10 = node0.AddChild("node10");
            var node20 = node0.AddChild("node20");

            var node11 = node10.AddChild("node11");

            var ancestors = node11.Ancestors();

            ancestors.Should().HaveCount(2);
            ancestors[0].Value.Should().Be(node10.Value);
            ancestors[1].Value.Should().Be(node0.Value);
        }

        [Fact]
        public void GetDescendants()
        {
            var node0 = new TreeNode<string>("node0");

            var node10 = node0.AddChild("node10");
            var node20 = node0.AddChild("node20");

            var node11 = node10.AddChild("node11");

            var descendants = node0.Descendants();

            descendants.Should().HaveCount(3);
            descendants[0].Value.Should().Be(node10.Value);
            descendants[1].Value.Should().Be(node11.Value);
            descendants[2].Value.Should().Be(node20.Value);
        }

        [Fact]
        public void TestRoot()
        {
            var node0 = new TreeNode<string>("node0");

            var node01 = node0.AddChild("node01");
            var node02 = node0.AddChild("node02");

            var node021 = node02.AddChild("node021");
            var node022 = node02.AddChild("node022");

            var node0221 = node022.AddChild("node0221");
            var node0222 = node022.AddChild("node0222");

            node0222.Root.Should().BeEquivalentTo(node0);

            node01.Root.Should().BeEquivalentTo(node0);

            node0.Root.Should().BeEquivalentTo(node0);
        }

        [Fact]
        public void TestPath()
        {
            var node0 = new TreeNode<string>("node0");

            var node01 = node0.AddChild("node01");
            var node02 = node0.AddChild("node02");

            var node011 = node01.AddChild("node011");

            var node021 = node02.AddChild("node021");
            var node022 = node02.AddChild("node022");

            var node0221 = node022.AddChild("node0221");
            var node0222 = node022.AddChild("node0222");

            var path0 = node0.Path;
            path0.Should().HaveCount(1);
            path0.Should().Equal(new List<int> { 1 });

            var path01 = node01.Path;
            path01.Should().HaveCount(2);
            path01.Should().Equal(new List<int> { 1, 1 });

            var path011 = node011.Path;
            path011.Should().HaveCount(3);
            path011.Should().Equal(new List<int> { 1, 1, 1 });

            var path02 = node02.Path;
            path02.Should().HaveCount(2);
            path02.Should().Equal(new List<int> { 1, 2 });

            var path021 = node021.Path;
            path021.Should().HaveCount(3);
            path021.Should().Equal(new List<int> { 1, 2, 1 });

            var path022 = node022.Path;
            path022.Should().HaveCount(3);
            path022.Should().Equal(new List<int> { 1, 2, 2 });

            var path0221 = node0221.Path;
            path0221.Should().HaveCount(4);
            path0221.Should().Equal(new List<int> { 1, 2, 2, 1 });

            var path0222 = node0222.Path;
            path0222.Should().HaveCount(4);
            path0222.Should().Equal(new List<int> { 1, 2, 2, 2 });

            var lst = node0.ToList();
        }

        [Fact]
        public void DepthFirstList()
        {
            var node0 = new TreeNode<string>("node0");

            var node01 = node0.AddChild("node01");
            var node02 = node0.AddChild("node02");

            var node011 = node01.AddChild("node011");

            var node021 = node02.AddChild("node021");
            var node022 = node02.AddChild("node022");

            var node0211 = node021.AddChild("node0211");
            var node0221 = node022.AddChild("node0221");
            var node0222 = node022.AddChild("node0222");

            var lst = node0.ToList(TreeTraverseOrder.DepthFirst);

            lst[0].ToString().Should().Be(node0.ToString());

            lst[1].ToString().Should().Be(node01.ToString());
            lst[2].ToString().Should().Be(node011.ToString());

            lst[3].ToString().Should().Be(node02.ToString());
            lst[4].ToString().Should().Be(node021.ToString());
            lst[5].ToString().Should().Be(node0211.ToString());

            lst[6].ToString().Should().Be(node022.ToString());
            lst[7].ToString().Should().Be(node0221.ToString());
            lst[8].ToString().Should().Be(node0222.ToString());
        }

        [Fact]
        public void BreadthFirstList()
        {
            var node0 = new TreeNode<string>("node0");

            var node01 = node0.AddChild("node01");
            var node02 = node0.AddChild("node02");

            var node011 = node01.AddChild("node011");

            var node021 = node02.AddChild("node021");
            var node022 = node02.AddChild("node022");

            var node0211 = node021.AddChild("node0211");
            var node0221 = node022.AddChild("node0221");
            var node0222 = node022.AddChild("node0222");

            var lst = node0.ToList(TreeTraverseOrder.BreadthFirst);

            lst[0].ToString().Should().Be(node0.ToString());

            lst[1].ToString().Should().Be(node01.ToString());
            lst[2].ToString().Should().Be(node02.ToString());

            lst[3].ToString().Should().Be(node011.ToString());
            lst[4].ToString().Should().Be(node021.ToString());
            lst[5].ToString().Should().Be(node022.ToString());

            lst[6].ToString().Should().Be(node0211.ToString());
            lst[7].ToString().Should().Be(node0221.ToString());
            lst[8].ToString().Should().Be(node0222.ToString());
        }

        [Fact]
        public void CanIterate()
        {
            var node0 = new TreeNode<string>("node0");

            var node01 = node0.AddChild("node01");
            var node02 = node0.AddChild("node02");

            var node011 = node01.AddChild("node011");

            var node021 = node02.AddChild("node021");
            var node022 = node02.AddChild("node022");

            var node0211 = node021.AddChild("node0211");
            var node0221 = node022.AddChild("node0221");
            var node0222 = node022.AddChild("node0222");

            var strings = new List<string>();

            foreach (var item in node0)
            {
                strings.Add(item.ToString());
            }

            strings[0].Should().Be(node0.ToString());

            strings[1].Should().Be(node01.ToString());
            strings[2].Should().Be(node011.ToString());

            strings[3].Should().Be(node02.ToString());
            strings[4].Should().Be(node021.ToString());

            strings[5].Should().Be(node0211.ToString());

            strings[6].Should().Be(node022.ToString());
            strings[7].Should().Be(node0221.ToString());
            strings[8].Should().Be(node0222.ToString());
        }

        [Fact]
        public void PathStringDefault()
        {
            var node1 = new TreeNode<string>("node1");

            var node11 = node1.AddChild("node11");
            var node111 = node11.AddChild("node111");

            node111.PathString.Should().Be("1.1.1");
        }

        [Fact]
        public void PathDefault()
        {
            var node0 = new TreeNode<string>("node0");

            var node01 = node0.AddChild("node01");
            var node011 = node01.AddChild("node011");

            var node02 = node0.AddChild("node02");
            var node021 = node02.AddChild("node021");
            var node0211 = node021.AddChild("node0211");
            var node0212 = node021.AddChild("node0212");

            var path = node0212.Path;

            path.Should().HaveCount(4);
            path.Should().ContainInOrder(new List<int> { 1, 2, 1, 2 });
        }

        [Fact]
        public void InheritedTypedTreeNode()
        {
            var node1 = new MyTypedTreeNode(new Dictionary<string, string> {
                { "Name", "node1" },
                { "ExpectedPath", "1" }
            }, MyTreeNodeType.Folder);

            var node1_1 = node1.AddChild(new MyTypedTreeNode(new Dictionary<string, string> {
                { "Name", "node1-1" },
                { "ExpectedPath", "1-1" }
            }, MyTreeNodeType.Leaf));

            var node1_2 = node1.AddChild(new MyTypedTreeNode(new Dictionary<string, string> {
                { "Name", "node1-2" },
                { "ExpectedPath", "1-2" }
            }, MyTreeNodeType.Leaf));

            var node11 = node1.AddChild(new MyTypedTreeNode(new Dictionary<string, string> {
                { "Name", "node1.1" },
                { "ExpectedPath", "1.1" }
            }, MyTreeNodeType.Folder));

            var node11_1 = node11.AddChild(new MyTypedTreeNode(new Dictionary<string, string> {
                { "Name", "node1.1-1" },
                { "ExpectedPath", "1.1-1" }
            }, MyTreeNodeType.Leaf));

            var node11_2 = node11.AddChild(new MyTypedTreeNode(new Dictionary<string, string> {
                { "Name", "node1.1-2" },
                { "ExpectedPath", "1.1-2" }
            }, MyTreeNodeType.Leaf));

            var node111 = node11.AddChild(new MyTypedTreeNode(new Dictionary<string, string> {
                { "Name", "node1.1.1" },
                { "ExpectedPath", "1.1.1" }
            }, MyTreeNodeType.Folder));

            var node111_1 = node111.AddChild(new MyTypedTreeNode(new Dictionary<string, string> {
                { "Name", "node1.1.1-1" },
                { "ExpectedPath", "1.1.1-1" }
            }, MyTreeNodeType.Leaf));

            var node111_2 = node111.AddChild(new MyTypedTreeNode(new Dictionary<string, string> {
                { "Name", "node1.1.1-2" },
                { "ExpectedPath", "1.1.1-2" }
            }, MyTreeNodeType.Leaf));

            var node12 = node1.AddChild(new MyTypedTreeNode(new Dictionary<string, string> {
                { "Name", "node1.2" },
                { "ExpectedPath", "1.2" }
            }, MyTreeNodeType.Folder));

            var nodes = node1.ToList();

            nodes.Should().HaveCount(10);

            foreach (var node in nodes)
            {
                node.PathString.Should().Be(node.Value["ExpectedPath"]);
            }
        }

        [Fact]
        public void InheritedGenericTreeNode()
        {
            var node1 = new MyGenericTreeNode<Dictionary<string, string>>(new Dictionary<string, string> {
                { "Name", "node1" },
                { "ExpectedPath", "1" }
            }, MyTreeNodeType.Folder);

            var node1_1 = node1.AddChild(new MyGenericTreeNode<Dictionary<string, string>>(new Dictionary<string, string> {
                { "Name", "node1-1" },
                { "ExpectedPath", "1-1" }
            }, MyTreeNodeType.Leaf));

            var node1_2 = node1.AddChild(new MyGenericTreeNode<Dictionary<string, string>>(new Dictionary<string, string> {
                { "Name", "node1-2" },
                { "ExpectedPath", "1-2" }
            }, MyTreeNodeType.Leaf));

            var node11 = node1.AddChild(new MyGenericTreeNode<Dictionary<string, string>>(new Dictionary<string, string> {
                { "Name", "node1.1" },
                { "ExpectedPath", "1.1" }
            }, MyTreeNodeType.Folder));

            var node11_1 = node11.AddChild(new MyGenericTreeNode<Dictionary<string, string>>(new Dictionary<string, string> {
                { "Name", "node1.1-1" },
                { "ExpectedPath", "1.1-1" }
            }, MyTreeNodeType.Leaf));

            var node11_2 = node11.AddChild(new MyGenericTreeNode<Dictionary<string, string>>(new Dictionary<string, string> {
                { "Name", "node1.1-2" },
                { "ExpectedPath", "1.1-2" }
            }, MyTreeNodeType.Leaf));

            var node111 = node11.AddChild(new MyGenericTreeNode<Dictionary<string, string>>(new Dictionary<string, string> {
                { "Name", "node1.1.1" },
                { "ExpectedPath", "1.1.1" }
            }, MyTreeNodeType.Folder));

            var node111_1 = node111.AddChild(new MyGenericTreeNode<Dictionary<string, string>>(new Dictionary<string, string> {
                { "Name", "node1.1.1-1" },
                { "ExpectedPath", "1.1.1-1" }
            }, MyTreeNodeType.Leaf));

            var node111_2 = node111.AddChild(new MyGenericTreeNode<Dictionary<string, string>>(new Dictionary<string, string> {
                { "Name", "node1.1.1-2" },
                { "ExpectedPath", "1.1.1-2" }
            }, MyTreeNodeType.Leaf));

            var node12 = node1.AddChild(new MyGenericTreeNode<Dictionary<string, string>>(new Dictionary<string, string> {
                { "Name", "node1.2" },
                { "ExpectedPath", "1.2" }
            }, MyTreeNodeType.Folder));

            var nodes = node1.ToList();

            nodes.Should().HaveCount(10);

            foreach (var node in nodes)
            {
                node.PathString.Should().Be(node.Value["ExpectedPath"]);
            }
        }
    
        [Fact]
        public void InsertBefore()
        {
            var node1 = new TreeNode<string>("node1");

            var node11 = node1.AddChild("node11");
            var node12 = node1.AddChild("node12");
            var node13 = node1.AddChild("node13");

            var insertedNode1 = node1.InsertBefore(node11, new TreeNode<string>("insertedNode1"));

            node1.Children.Should().HaveCount(4);
            node1.Children[0].Should().BeEquivalentTo(insertedNode1);
            node1.Children[1].Should().BeEquivalentTo(node11);

            var insertedNode2 = node1.InsertBefore(node13, new TreeNode<string>("insertedNode2"));

            node1.Children.Should().HaveCount(5);
            node1.Children[0].Should().BeEquivalentTo(insertedNode1);
            node1.Children[1].Should().BeEquivalentTo(node11);
            node1.Children[3].Should().BeEquivalentTo(insertedNode2);
            node1.Children[4].Should().BeEquivalentTo(node13);
        }

        [Fact]
        public void InsertAfter()
        {
            var node1 = new TreeNode<string>("node1");

            var node11 = node1.AddChild("node11");
            var node12 = node1.AddChild("node12");
            var node13 = node1.AddChild("node13");

            var insertedNode1 = node1.InsertAfter(node11, new TreeNode<string>("insertedNode1"));

            node1.Children.Should().HaveCount(4);
            node1.Children[0].Should().BeEquivalentTo(node11);
            node1.Children[1].Should().BeEquivalentTo(insertedNode1);

            var insertedNode2 = node1.InsertAfter(node13, new TreeNode<string>("insertedNode2"));

            node1.Children.Should().HaveCount(5);
            node1.Children[0].Should().BeEquivalentTo(node11);
            node1.Children[1].Should().BeEquivalentTo(insertedNode1);
            node1.Children[3].Should().BeEquivalentTo(node13);
            node1.Children[4].Should().BeEquivalentTo(insertedNode2);
        }

        [Fact]
        public void InsertBelow()
        {
            var node1 = new TreeNode<string>("node1");

            var node11 = node1.AddChild("node11");
            var node12 = node1.AddChild("node12");
            var node13 = node1.AddChild("node13");

            node1.Children.Should().HaveCount(3);
            node11.Children.Should().HaveCount(0);

            var insertedNode1 = node1.InsertBelow(node11, new TreeNode<string>("insertedNode1"));

            node1.Children.Should().HaveCount(3);
            node11.Children.Should().HaveCount(1);
            node11.Children[0].Should().BeEquivalentTo(insertedNode1);
        }

        [Fact]
        public void InsertFailsWithNodeNotFound()
        {
            var node1 = new TreeNode<string>("node1");

            var node11 = node1.AddChild("node11");
            var node12 = node1.AddChild("node12");
            var node13 = node1.AddChild("node13");

            var invalidNode = new TreeNode<string>("invalid");

            node1.Invoking(x => x.InsertBefore(invalidNode, new TreeNode<string>("insertedNode1")))
                .Should().Throw<ArgumentOutOfRangeException>();
        }


        [Fact]
        public void RemoveSelf()
        {
            var node1 = new TreeNode<string>("node1");

            var node11 = node1.AddChild("node11");
            var node12 = node1.AddChild("node12");
            var node13 = node1.AddChild("node13");

            node1.Children.Should().HaveCount(3);
            node1.Children.Contains(node12).Should().BeTrue();

            node12.Remove();

            node1.Children.Should().HaveCount(2);
            node1.Children.Contains(node12).Should().BeFalse();

            node1.Children.Contains(node13).Should().BeTrue();

            node1.Remove(node13);

            node1.Children.Should().HaveCount(1);
            node1.Children.Contains(node13).Should().BeFalse();
        }

        [Fact]
        public void Siblings()
        {
            var node1 = new TreeNode<string>("node1");

            var node11 = node1.AddChild("node11");
            var node12 = node1.AddChild("node12");
            var node13 = node1.AddChild("node13");
            var node14 = node1.AddChild("node14");
            var node15 = node1.AddChild("node15");

            node1.Children.Should().HaveCount(5);

            var siblings = node13.Siblings();

            siblings.Should().HaveCount(4);
            siblings.Should().NotContain(node13);
        }

        [Fact]
        public void Predecessors()
        {
            var node1 = new TreeNode<string>("node1");

            var node11 = node1.AddChild("node11");
            var node12 = node1.AddChild("node12");
            var node13 = node1.AddChild("node13");
            var node14 = node1.AddChild("node14");
            var node15 = node1.AddChild("node15");

            node1.Children.Should().HaveCount(5);

            var predecessors1 = node13.Predecessors();

            predecessors1.Should().HaveCount(2);
            predecessors1.Should().ContainInOrder(node11, node12);

            var predecessors2 = node15.Predecessors();

            predecessors2.Should().HaveCount(4);
            predecessors2.Should().ContainInOrder(node11, node12, node13, node14);

            var predecessors3 = node11.Predecessors();

            predecessors3.Should().HaveCount(0);
        }

        [Fact]
        public void Successors()
        {
            var node1 = new TreeNode<string>("node1");

            var node11 = node1.AddChild("node11");
            var node12 = node1.AddChild("node12");
            var node13 = node1.AddChild("node13");
            var node14 = node1.AddChild("node14");
            var node15 = node1.AddChild("node15");

            node1.Children.Should().HaveCount(5);

            var successors1 = node13.Successors();

            successors1.Should().HaveCount(2);
            successors1.Should().ContainInOrder(node14, node15);

            var successors2 = node11.Successors();

            successors2.Should().HaveCount(4);
            successors2.Should().ContainInOrder(node12, node13, node14, node15);

            var successors3 = node15.Successors();

            successors3.Should().HaveCount(0);
        }
    }
}
