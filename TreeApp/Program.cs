using System;
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
    


    public interface IBinaryTreeNode<T> where T : IComparable
    {
        void Add(T value);
        bool Contains(T value);
        void Remove(T value);
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

        public void Add3(T value)
        {
            var node = new Node<T>(value);

            if (_head == null)
            {
                _head = node;

            }
            else
            {
                INode<T> currentNode = _head;

                while (currentNode != null)
                {
                    if (currentNode.Value.CompareTo(value) == 0) return;

                    if (currentNode.Value.CompareTo(value) < 0)
                    {
                        if (currentNode.Right == null)
                        {
                            currentNode.Right = node;
                            break;
                        }
                        currentNode = currentNode.Right;
                    }

                    else
                    {
                        if (currentNode.Left == null)
                        {
                            currentNode.Left = node;
                            break;
                        }
                        currentNode = currentNode.Left;
                    }

                }
            }
            _count++;
        }
        public void Add2(T value)
        {
            var node = new Node<T>(value);

            if (_head == null)
            {
                _head = node;

            }

            else
            {
                var parentNode = tryFindNodeParent(_head, value);

                if (parentNode != null)
                {
                    if (parentNode.Value.CompareTo(value) < 0)
                    {
                        parentNode.Right = node;
                        _count++;
                    } else
                    {
                        parentNode.Left = node;
                        _count++;
                    }
                }
            }

        }
        public void Add(T value)
        {
            Node<T> node = new Node<T>(value);
            INode<T> current = null;

            if (_head == null)
            {
                _head = node;                
                current = _head;
            } else
            {
                if (current.Value.CompareTo(value) < 0)
                {
                    if (current.Right == null)
                    { current.Right = node; current = _head; }

                    else
                    {
                        current = current.Right;
                        Add(value);
                    }
                }

                else if (current.Value.CompareTo(value) > 0)
                {
                    if (current.Left == null)
                    { current.Left = node; current = _head; }

                    else
                    {
                        current = current.Left;
                        Add(value);
                    }
                }

                else return;
            }
            _count++;
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

        private INode<T> findMinimumLeftNodeParent(INode<T> tree)
        {
            if (tree == null)
            {
                return null;
            }

            if (tree.Left != null && tree.Left.Left == null)
            {
                return tree;
            }

            return findMinimumLeftNodeParent(tree.Left);
        }

        private INode<T> findMaximumRightNodeParent(INode<T> tree)
        {
            if (tree == null)
            {
                return null;
            }

            if (tree.Right != null && tree.Right.Right == null)
            {
                return tree;
            }

            return findMaximumRightNodeParent(tree.Right);
        }

        public void Remove(T value)
        {
            INode<T> parent = null;
            INode<T> current = _head;

            while (current != null)
            {
                if(current.Value.CompareTo(value) == 0)
                {
                    break;
                }

                if (current.Value.CompareTo(value) < 0)
                {
                    parent = current;
                    current = current.Right;
                } else 
                {
                    parent = current;
                    current = current.Left;
                }               
            }

            if (current == null)
            {
                Console.WriteLine("There is no such an element in the tree!!!");
                return;
            }

            if (current.Left == null && current.Right == null)
            {
                if(parent == null)
                {
                    _head = null;
                } else
                if (parent.Value.CompareTo(current.Value) < 0)
                {
                    parent.Right = null;
                } else 
                {
                    parent.Left = null;
                } 
               
            } else if (current.Left == null && current.Right != null)
            {
                if (parent == null)
                {


                    var minimumNodeParent = findMinimumLeftNodeParent(current.Right);
                    if (minimumNodeParent != null)
                    {
                        var minimumLeftNode = minimumNodeParent.Left;
                        minimumNodeParent.Left = null;
                        current.Value = minimumLeftNode.Value;

                    }
                } else if (parent.Value.CompareTo(current.Value) < 0)
                {
                    var minimumNodeParent = findMinimumLeftNodeParent(current.Right);
                    if (minimumNodeParent != null)
                    { 
                        var minimumLeftNode = minimumNodeParent.Left;
                        minimumNodeParent.Left = null;
                        parent.Right = minimumLeftNode;
                    }
                    else
                    {
                        parent.Right = current.Right;
                    }
                } else
                {
                    var minimumNodeParent = findMinimumLeftNodeParent(current.Right);
                    if (minimumNodeParent != null)
                    {
                        var minimumLeftNode = minimumNodeParent.Left;
                        minimumNodeParent.Left = null;
                        parent.Left = minimumLeftNode;
                    }
                    else
                    {
                        parent.Left = current.Right;
                    }
                }
            } else if (current.Left != null && current.Right == null)
            {
                if (parent == null)
                {


                    var maximumNodeParent = findMaximumRightNodeParent(current.Left);
                    if (maximumNodeParent != null)
                    {
                        var minimumLeftNode = maximumNodeParent.Right;
                        maximumNodeParent.Right = null;
                        current.Value = minimumLeftNode.Value;

                    }
                } else if (parent.Value.CompareTo(current.Value) < 0)
                {
                    var maximumNodeParent = findMaximumRightNodeParent(current.Left);
                    if (maximumNodeParent != null)
                    {
                        var maximumRightNode = maximumNodeParent.Right;
                        maximumRightNode.Right = null;
                        parent.Right = maximumRightNode;
                    }
                    else
                    {
                        parent.Right = current.Right;
                    }
                }
                else
                {
                    var maximumNodeParent = findMaximumRightNodeParent(current.Left);
                    if (maximumNodeParent != null)
                    {
                        var maximumRightNode = maximumNodeParent.Right;
                        maximumNodeParent.Right = null;
                        parent.Left = maximumRightNode;
                    }
                    else
                    {
                        parent.Left = current.Right;
                    }
                }
            } else if(current.Left != null && current.Right != null)
            {
                if(parent == null) 
                {
                   
                    
                    var minimumNodeParent = findMinimumLeftNodeParent(current.Right);
                    if (minimumNodeParent != null)
                    {
                        var minimumLeftNode = minimumNodeParent.Left;
                        minimumNodeParent.Left = null;
                        current.Value = minimumLeftNode.Value;

                    } 
                } else if (parent.Value.CompareTo(current.Value) < 0)
                {
                    var minimumNodeParent = findMinimumLeftNodeParent(current.Right);
                    if (minimumNodeParent != null)
                    {
                        var minimumLeftNode = minimumNodeParent.Left;
                        minimumNodeParent.Left = null;
                        parent.Right = minimumLeftNode;
                    }
                    else
                    {
                        parent.Right = current.Right;
                    }
                }
                else
                {
                    var minimumNodeParent = findMinimumLeftNodeParent(current.Right);
                    if (minimumNodeParent != null)
                    {
                        var minimumLeftNode = minimumNodeParent.Left;
                        minimumNodeParent.Left = null;
                        parent.Left = minimumLeftNode;
                    }
                    else
                    {
                        parent.Left = current.Right;
                    }
                }
            }
            _count--;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BinaryTreeNode<int> tree = new BinaryTreeNode<int>();
            tree.Add2(6);
            tree.Add2(8);
            tree.Add2(4);
            tree.Add2(5);
            tree.Add2(2);
            tree.Add2(1);
            //tree.Add2(2);

            tree.Remove(8);
            tree.Remove(6);


            Console.WriteLine(tree.Contains(10));
            Console.WriteLine(tree.Contains(1));

            tree.Remove(4);

            tree.Clear();

            // Console.WriteLine("Output");

            Console.ReadLine();
        }
    }

}