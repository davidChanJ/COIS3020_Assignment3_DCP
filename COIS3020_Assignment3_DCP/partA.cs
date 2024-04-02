using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS3020_Assignment3_DCP
{
    public class Node
    {
        public Node[] branches { get; set; }
        public int[] value { get; set; }

        // Constructor
        public Node(int[] value)
        {
            // value.Length means the number of dimension
            // number of branches will be 2 ^ value.Length
            branches = new Node[(int)Math.Pow(2, value.Length)];
            this.value = value;
        }
    }

    public class QuadTree
    {
        private Node root;
        //private List<Node> branches;

        public QuadTree()
        {
            root = null;
        }

        public QuadTree(int[] value)
        {
            root = new Node(value);
        }

        public static int PointDirection(QuadTree currentTree, QuadTree otherTree)
        {
            // check is two tree have the same dimension
            int d = currentTree.root.value.Length;
            int d2 = otherTree.root.value.Length;
            if (d != d2)
            {
                return -1;
            }

            // find index for otherTree
            int index = 0;
            // loop over the entire currentTree
            for (int i = 0; i < d; i++)
            {
                /**
                 * if value of the currentTree[i] is smaller than the value of the other tree[i]
                 * 
                 * d stands for the number of dimension of the branches
                 * max value of index = (2 ^ d) - 1
                 * min value of index = 0
                 * 
                 *  We are locating the index to return by breaking the branch by 2 recusrively,
                 * i.e., when i = 0, we are comparing the first index of the currentTree value and the otherTree value
                 * if currentTree value < otherTree value, we increment index by 2 ^ (d - i - 1),
                 * which mean we are looking at the latter half of the branch, upper half otherwise.
                 * By doing this recursively, we locate the index we wouls like to place the otherTree in
                 * 
                 * for example, if d = 3, currentTree root value = [0,0,0], otherTree root value = [2, -1, 2]
                 * first compare the first index, 0 < 2, index += 2^(3-0-1) = 4
                 * then compare the second index, 0 > -1, do nothing on index
                 * third compare the third index, 0 < 2, index += 2^(3-2-1) = 5
                 * return 5
                 */

                if (currentTree.root.value[i] < otherTree.root.value[i])
                {
                    index += (int)Math.Pow(2, d - i - 1);
                }
            }

            return index;
        }

    }
}
