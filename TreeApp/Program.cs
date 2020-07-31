using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace TreeApp
{

    public interface INode<T> : IComparable where T : IComparable
    {
        INode<T> Left { get; set; }
        INode<T> Right { get; set; }

        T Value { get; set; }
    }

    public interface ITreeTravelsar<T> where T : IComparable
    {
        void Print(INode<T> node, IBinaryTreeNode<T> tree);
    }

    internal class InOrder<T> : ITreeTravelsar<T> where T : IComparable
    {
        public void Print(INode<T> node, IBinaryTreeNode<T> tree)
        {
            if(node != null)
            {
                Print(node.Left, tree);
                Console.Write(node.Value + " ");
                Print(node.Right, tree);
            }
        }
    }

    internal class PreOrder<T> : ITreeTravelsar<T> where T : IComparable
    {
        public void Print(INode<T> node, IBinaryTreeNode<T> tree)
        {
            if (node != null)
            {
                Console.Write(node.Value + " ");
                Print(node.Left, tree);
                Print(node.Right, tree);
            }
        }
    }

    internal class PostOrder<T> : ITreeTravelsar<T> where T : IComparable
    {
        public void Print(INode<T> node, IBinaryTreeNode<T> tree)
        {
            if (node != null)
            {
                Print(node.Left, tree);
                Print(node.Right, tree);
                Console.Write(node.Value + " ");
            }
        }
    }


    public interface IBinaryTreeNode<T> where T : IComparable
    {
        void Add2(T value);
        bool Contains(T value);
        INode<T> Remove(INode<T> tree, T value);
        void Clear();
        int Count { get; }
    }

    internal class Node<T> : INode<T> where T : IComparable
    {
        public Node(T v)
        { Value = v; }
        public INode<T> Left { get; set; }
        public INode<T> Right { get; set; }
        public T Value { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is Node<T> itm)
                return Value.CompareTo(itm.Value);
            else throw new ArgumentException("Types");
        }

        
    }

    internal class BinaryTreeNode<T> : IBinaryTreeNode<T> where T : IComparable
    {
        private Node<T> _head;
        private int _count = 0;


        public int Count { get { return _count; } }

        public INode<T> GetHead()
        {
            if (_head == null) return null;
            return _head;
        }

        public INode<T> tryFindNodeParent(INode<T> node, T value)
        {
            if (node.Value.CompareTo(value) < 0)
            {
                if (node.Right == null)
                {
                    return node;
                }
                return tryFindNodeParent(node.Right, value);
            }
            else if (node.Value.CompareTo(value) > 0)
            {
                if (node.Left == null)
                {
                    return node;
                }

                return tryFindNodeParent(node.Left, value);
            }
            else return null;
        }

        public void Add2(T value)
        {
            var node = new Node<T>(value);

            if (_head == null)
            {
                _head = node;
                _count++;

            } else
            {
                var parentNode = tryFindNodeParent(_head, value);

                if (parentNode != null)
                {
                    if (parentNode.Value.CompareTo(value) < 0)
                    {
                        parentNode.Right = node;
                        _count++;
                    }
                    else
                    {
                        parentNode.Left = node;
                        _count++;
                    }
                }
            }
        }
        
        public void Clear()
        {
            _head = null;
            _count = 0;
        }

        public bool Contains(T value)
        {
            INode<T> current = _head;
            while (current != null)
            {
                if (current.Value.CompareTo(value) == 0)
                {
                    return true;
                }

                current = current.Value.CompareTo(value) < 0
                        ? current.Right
                        : current.Left;

            }
            return false;
        }


        private INode<T> findMinimumNode(INode<T> tree)
        {
            if (tree == null)
            {
                return null;
            }

            if (tree.Left == null)
            {
                return tree;
            }

            return findMinimumNode(tree.Left);
        }

        public INode<T> Remove(INode<T> tree, T value)
        {

            if(tree == null)
            {
                return tree;
            }

            if((value.CompareTo(tree.Value)) < 0)
            {
                tree.Left = Remove(tree.Left, value);
            } else
            if ((value.CompareTo(tree.Value)) > 0)
            {
                tree.Right = Remove(tree.Right, value);
            } else
            if (tree.Left != null && tree.Right != null)
            {
                tree.Value = findMinimumNode(tree.Right).Value;
                tree.Right = Remove(tree.Right, tree.Value);
            } else
            {
                if (tree.Left != null)
                {
                    tree = tree.Left;
                } else 
                if (tree.Right != null)
                {
                    tree = tree.Right;
                } else
                {
                    tree = null;
                }
            }
            return tree;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BinaryTreeNode<int> tree = new BinaryTreeNode<int>();
            //InOrder<int> trvl = new InOrder<int>();
            tree.Add2(6);
            tree.Add2(8);
            tree.Add2(4);
            tree.Add2(5);
            tree.Add2(2);
            tree.Add2(1);
            //tree.Add2(2);

            //trvl.Print(tree.GetHead(), tree);

            List<ITreeTravelsar<int>> list = new List<TreeApp.ITreeTravelsar<int>>() {new InOrder<int>(), new PreOrder<int>(), new PostOrder<int>()};
            foreach(var pth in list)
            {
                pth.Print(tree.GetHead(), tree);
                Console.WriteLine();
            }
            Console.WriteLine(tree.Contains(5));
            Console.WriteLine(tree.Contains(1));

            

            tree.Clear();

            // Console.WriteLine("Output");

            Console.ReadLine();
        }
    }

}