using BSTforRBTree;
using System;
namespace COIS3020_Assignment3_DCP
{
    public class TwoThreeFourTree<T> where T : IComparable<T>
    {
        public class Node
        {
            public List<T> Keys { get; set; } = new List<T>();
            public List<Node> Children { get; set; } = new List<Node>();

            public Node(T k)
            {
                Keys.Add(k);
            }

            public bool isLeafNode()
            {
                if (Children.Count == 0)
                {
                    return true;
                }
                return false;
            }
        }

        private Node root;
        const int maxKeys = 3;

        // constructor
        public TwoThreeFourTree()
        {
            root = null;
        }

        public bool Insert(T k)
        {
            // when root is empty
            if (root == null)
            {
                root = new Node(k);
                return true;
            }

            // find path to k or fullnode along the path to k
            List<Node> path = PathToKeyOrFullNode(k);
            Node lastNode;

            while (path[path.Count - 1].Keys.Count == maxKeys)
            {
                lastNode = path[path.Count - 1];

                // split and move up
                if (lastNode.Keys.Count == maxKeys)
                {
                    Node leftNode = new Node(lastNode.Keys[0]);
                    Node middleNode = new Node(lastNode.Keys[1]);
                    Node rightNode = new Node(lastNode.Keys[2]);

                    // handle child for internal node
                    if (!lastNode.isLeafNode())
                    {
                        leftNode.Children.Add(lastNode.Children[0]);
                        leftNode.Children.Add(lastNode.Children[1]);
                        rightNode.Children.Add(lastNode.Children[2]);
                        rightNode.Children.Add(lastNode.Children[3]);
                    }

                    middleNode.Children.Add(leftNode);
                    middleNode.Children.Add(rightNode);

                    // insert middleNode to its parent
                    if (path.Count == 1)
                    {
                        root = middleNode;
                    }
                    else
                    {
                        // node to contain the ascend node
                        Node parentNode = path[path.Count - 2];
                        int j;
                        for (j = 0; j < parentNode.Keys.Count; j++)
                        {
                            if (middleNode.Keys[0].CompareTo(parentNode.Keys[j]) < 0)
                            {
                                parentNode.Keys.Insert(j, middleNode.Keys[0]);
                                parentNode.Children.RemoveAt(j);
                                parentNode.Children.Insert(j, leftNode);
                                parentNode.Children.Insert(j + 1, rightNode);
                            }
                        }
                        if (j == parentNode.Keys.Count)
                        {
                            parentNode.Keys.Add(middleNode.Keys[0]);
                            parentNode.Children.RemoveAt(j);
                            parentNode.Children.Insert(j, leftNode);
                            parentNode.Children.Insert(j + 1, rightNode);

                        }
                    }

                    // recalculate path to key k
                    path = PathToKeyOrFullNode(k);
                }
            }

            // add to leaf node
            Node leafNode = path[path.Count - 1];
            int i;
            for (i = 0; i < leafNode.Keys.Count; i++)
            {
                // k is smaller than current key, insert k at i
                if (k.CompareTo(leafNode.Keys[i]) < 0)
                {
                    leafNode.Keys.Insert(i, k);
                    return true;
                }

                // duplicate key in leaf node, return false
                if (k.CompareTo(leafNode.Keys[i]) == 0)
                {
                    return false;
                }
            }

            // k is greater than all keys in leaf node
            if (i == leafNode.Keys.Count)
            {
                leafNode.Keys.Insert(i, k);
                return true;
            }

            return false;
        }

        // function to find path to k or path to k which stopped at a full node
        public List<Node> PathToKeyOrFullNode(T k)
        {
            List<Node> path = new List<Node>();
            Node current = root;
            // stop looping when node is a leaf node or full node
            while (!current.isLeafNode() && current.Keys.Count != maxKeys)
            {
                path.Add(current);
                int i;
                int currentKeyCount = current.Keys.Count;
                // loop over keys of current
                for (i = 0; i < currentKeyCount; i++)
                {
                    // k is smaller than key, move current to the child
                    if (k.CompareTo(current.Keys[i]) < 0)
                    {
                        current = current.Children[i];
                        break;
                    }
                }
                // move current to child if k is bigger than all value in current node
                if (i == currentKeyCount)
                {
                    current = current.Children[i];
                }
            }
            // add leaf node or full node to path
            path.Add(current);
            return path;
        }

        public bool Delete(T k)
        {
            // return false if root is empty
            if (root == null)
            {
                return false;
            }

            List<Node> path = PrepareForDelete(k);

            Node lastNode = path[path.Count - 1];
            // k is in leaf node
            if (lastNode.isLeafNode())
            {
                int kIndex = lastNode.Keys.IndexOf(k);
                if (kIndex >= 0)
                {
                    lastNode.Keys.RemoveAt(kIndex);
                    return true;
                }
            }
            // k is in internal node
            else
            {
                int kIndex = lastNode.Keys.IndexOf(k);
                // search for k's successor
                T successor;
                Node temp = lastNode.Children[kIndex + 1];
                while (!temp.isLeafNode())
                {
                    temp = temp.Children[0];
                }
                successor = temp.Keys[0];
                // replace k by successor
                if (temp.Keys.Count > 1)
                {
                    lastNode.Keys[kIndex] = successor;
                    temp.Keys.RemoveAt(0);
                    return true;
                }

                // search for k's predecessor
                T predecessor;
                Node temp2 = lastNode.Children[kIndex];
                while (!temp2.isLeafNode())
                {
                    temp2 = temp2.Children[temp2.Children.Count - 1];
                }
                predecessor = temp2.Keys[temp2.Keys.Count - 1];
                // replace k by predecessor
                if (temp2.Keys.Count > 1)
                {
                    lastNode.Keys[kIndex] = predecessor;
                    temp2.Keys.RemoveAt(temp2.Keys.Count - 1);
                    return true;
                }

                // both successor and predecessor node have only one key
                // run PrepareForDelete to make one of them have two key
                List<Node> pathToSuccessor = PrepareForDelete(successor);
                Node newSuccessorNode = pathToSuccessor[pathToSuccessor.Count - 1];
                if (newSuccessorNode.isLeafNode() && newSuccessorNode.Keys.Count > 1)
                {
                    lastNode.Keys[kIndex] = newSuccessorNode.Keys[0];
                    newSuccessorNode.Keys.RemoveAt(0);
                    return true;
                }

                List<Node> pathToPredecessor = PrepareForDelete(predecessor);
                Node newPredecessorNode = pathToPredecessor[pathToPredecessor.Count - 1];
                if (newPredecessorNode.isLeafNode() && newPredecessorNode.Keys.Count > 1)
                {
                    lastNode.Keys[kIndex] = newPredecessorNode.Keys[0];
                    newPredecessorNode.Keys.RemoveAt(0);
                    return true;
                }
            }


            return false;
        }

        // Descend along the path based on the given key k from the root until the key is found.
        // Ensure each node along the path though must have at least 2 keys.
        // if less than 2 keys, make it be at least 2 keys by borrow or merge with sibiling as specified in lecture notes
        public List<Node> PrepareForDelete(T k)
        {
            // get path to key or node need to shrink (node have less than 2 keys)
            List<Node> path;
            bool end;
            (path, end) = PathToKeyOrShrinkNode(k);

            // loop until leaf node is reached or key k is located in internal node
            while (!end)
            {
                // shrink internal node
                Node parent = path[path.Count - 2];
                Node shrinkNode = path[path.Count - 1];
                // get childIndex of shrinkNode for parent
                int childIndex;
                for (childIndex = 0; childIndex < parent.Children.Count; childIndex++)
                {
                    if (parent.Children[childIndex] == shrinkNode)
                    {
                        break;
                    }
                }
                // try to borrow from left sibiling
                if (childIndex != 0 && parent.Children[childIndex - 1].Keys.Count > 2)
                {
                    Node leftSibiling = parent.Children[childIndex - 1];
                    int leftSibilingKeySize = leftSibiling.Keys.Count;
                    int leftSibilingChildrenSize = leftSibiling.Children.Count;
                    shrinkNode.Keys.Insert(0, parent.Keys[childIndex - 1]);
                    // get leftSibiling rightmost child to shrinkNode
                    if (!leftSibiling.isLeafNode())
                    {
                        shrinkNode.Children.Insert(0, leftSibiling.Children[leftSibilingChildrenSize - 1]);
                        leftSibiling.Children.RemoveAt(leftSibilingChildrenSize - 1);
                    }
                    parent.Keys[childIndex - 1] = leftSibiling.Keys[leftSibilingKeySize - 1];
                    leftSibiling.Keys.RemoveAt(leftSibilingKeySize - 1);
                }

                // try to borrow from right sibiling
                else if (childIndex != parent.Children.Count - 1 && parent.Children[childIndex + 1].Keys.Count > 2)
                {
                    Node rightSibiling = parent.Children[childIndex + 1];

                    // append right sibiling first key 
                    shrinkNode.Keys.Add(parent.Keys[childIndex]);
                    if (!rightSibiling.isLeafNode())
                    {
                        shrinkNode.Children.Add(rightSibiling.Children[0]);
                        rightSibiling.Children.RemoveAt(0);
                    }
                    parent.Keys[childIndex] = rightSibiling.Keys[0];
                    rightSibiling.Keys.RemoveAt(0);
                }
                // merge with adjacent node
                else
                {
                    // merge with left sibiling
                    if (childIndex != 0)
                    {
                        // merging leftSibiling with parent key and shrinkNode
                        Node leftSibiling = parent.Children[childIndex - 1];
                        leftSibiling.Keys.Add(parent.Keys[childIndex - 1]);
                        for (int i = 0; i < shrinkNode.Keys.Count; i++)
                        {
                            leftSibiling.Keys.Add(shrinkNode.Keys[i]);
                        }
                        if (!shrinkNode.isLeafNode())
                        {
                            for (int i = 0; i < shrinkNode.Children.Count; i++)
                            {
                                leftSibiling.Children.Add(shrinkNode.Children[i]);
                            }
                        }
                        // root has only one element, replace root by leftSibiling
                        if (path.Count < 3 && root.Keys.Count == 1)
                        {
                            root = leftSibiling;
                        }
                        else
                        {
                            parent.Keys.RemoveAt(childIndex - 1);
                            parent.Children[childIndex - 1] = leftSibiling;
                            parent.Children.RemoveAt(childIndex);
                        }
                    }
                    // merge with right sibiling
                    else if (childIndex != parent.Children.Count - 1)
                    {
                        // mergin shrinkNode with parent Key and rightSibiling
                        Node rightSibiling = parent.Children[childIndex + 1];
                        shrinkNode.Keys.Add(parent.Keys[childIndex]);
                        for (int i = 0; i < rightSibiling.Keys.Count; i++)
                        {
                            shrinkNode.Keys.Add(rightSibiling.Keys[i]);
                        }
                        if (!rightSibiling.isLeafNode())
                        {
                            for (int i = 0; i < rightSibiling.Children.Count; i++)
                            {
                                shrinkNode.Children.Add(rightSibiling.Children[i]);
                            }
                        }
                        // root has only one element, replace root by shrinkNode
                        if (path.Count < 3 && root.Keys.Count == 1)
                        {
                            root = shrinkNode;
                        }
                        else
                        {
                            parent.Keys.RemoveAt(childIndex);
                            parent.Children[childIndex] = shrinkNode;
                            parent.Children.RemoveAt(childIndex + 1);
                        }
                    }
                }
                (path, end) = PathToKeyOrShrinkNode(k);
            }
            return path;
        }

        public (List<Node>, bool) PathToKeyOrShrinkNode(T k)
        {
            List<Node> path = new List<Node>();
            Node current = root;
            // flag to see is the entire path to key k is explored
            bool end = false;

            // stop looping when node is a leaf node
            while (!current.isLeafNode())
            {
                if (current.Keys.Count < 2 && current != root)
                {
                    break;
                }
                int i;
                int currentKeyCount = current.Keys.Count;
                for (i = 0; i < currentKeyCount; i++)
                {
                    if (k.CompareTo(current.Keys[i]) < 0)
                    {
                        current = current.Children[i];
                        break;
                    }
                    // the key exist in internal node
                    if (k.CompareTo(current.Keys[i]) == 0)
                    {
                        end = true;
                        break;
                    }

                }
                if (i == currentKeyCount)
                {
                    current = current.Children[i];
                }
            }

            path.Add(current);
            if (current.isLeafNode() && (current.Keys.Count > 1 || current == root))
            {
                end = true;
            }

            return (path, end);
        }

        public bool Search(T k)
        {
            Node current = root;

            while (!current.isLeafNode())
            {
                int i;
                for (i = 0; i < current.Keys.Count; i++)
                {
                    if (k.CompareTo(current.Keys[i]) < 0)
                    {
                        current = current.Children[i];
                        break;
                    }
                    if (k.CompareTo(current.Keys[i]) == 0)
                    {
                        return true;
                    }

                }
                if (i == current.Keys.Count)
                {
                    current = current.Children[i];
                }
            }
            if (current.Keys.Contains(k))
            {
                return true;
            }

            return false;
        }

        public void Print()
        {
            if (root == null)
            {
                Console.WriteLine("Tree is empty.");
            }
            else
            {
                PrintInorder(root);
            }
        }

        private void PrintInorder(Node node)
        {
            if (node != null)
            {
                for (int i = 0; i < node.Keys.Count; i++)
                {
                    if (!node.isLeafNode())
                    {
                        PrintInorder(node.Children[i]);
                    }
                   
                    Console.Write(node.Keys[i] + " ");
                }
                if (!node.isLeafNode())
                {
                    PrintInorder(node.Children[node.Keys.Count]);
                }
            }
        }

        //public BSTforRBTree<T> Convert()
        //{
        //    BSTforRBTree<T> rbTree = new BSTforRBTree<T>();
        //    rbTree.root = ConvertToRBTree(root);
        //    return rbTree;
        //}

        //private BSTforRBTree<T>.Node ConvertToRBTree(Node node)
        //{
        //    if (node == null)
        //        return null;S

        //    BSTforRBTree<T>.Node rbNode = new BSTforRBTree<T>.Node(node.Keys[0], Color.BLACK);

        //    if (node.Keys.Count == 2)
        //    {
        //        BSTforRBTree<T>.Node rbRightNode = new BSTforRBTree<T>.Node(node.Keys[1], Color.RED);
        //        rbNode.Right = rbRightNode;
        //        rbNode.Left = ConvertToRBTree(node.Children[0]);
        //        rbRightNode.Left = ConvertToRBTree(node.Children[1]);
        //        rbRightNode.Right = ConvertToRBTree(node.Children[2]);
        //    }
        //    else if (node.Keys.Count == 3)
        //    {
        //        BSTforRBTree<T>.Node rbMiddleNode = new BSTforRBTree<T>.Node(node.Keys[1], Color.RED);
        //        BSTforRBTree<T>.Node rbRightNode = new BSTforRBTree<T>.Node(node.Keys[2], Color.RED);
        //        rbNode.Right = rbMiddleNode;
        //        rbMiddleNode.Right = rbRightNode;
        //        rbNode.Left = ConvertToRBTree(node.Children[0]);
        //        rbMiddleNode.Left = ConvertToRBTree(node.Children[1]);
        //        rbRightNode.Left = ConvertToRBTree(node.Children[2]);
        //        rbRightNode.Right = ConvertToRBTree(node.Children[3]);
        //    }
        //    else
        //    {
        //        rbNode.Left = ConvertToRBTree(node.Children[0]);
        //        rbNode.Right = ConvertToRBTree(node.Children[1]);
        //    }

        //    return rbNode;
        //}
    }
}
