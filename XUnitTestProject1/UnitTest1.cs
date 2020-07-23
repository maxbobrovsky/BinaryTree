using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            INode<int> node = new Node<int>(5);
            node.Right = new Node<int>(3);
            node.Left = new Node<int>(4);

            Assert.Equal(5, node.Value );
            Assert.Equal(3, node.Right.Value);
            Assert.Equal(4, node.Left.Value);
        }
    }

    internal class Node<T> : INode<int>
    {
        public int Value { get; set; }

        public Node(int v)
        {
            Value = v;
        }

        public Node<int> Right { get; set; }
        public Node<int> Left { get; set; }
    }

    internal interface INode<T>
    {
        Node<T> Right { get; set; }
        Node<T> Left { get; set; }
        T Value { get; set; }
    }
}
