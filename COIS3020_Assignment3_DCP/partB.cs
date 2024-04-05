using System;
namespace DavidDataStructureAssignment3
{
    public class BinomialNode
    {
        public int Item { get; set; }
        public int Degree { get; set; }
        public BinomialNode LeftMostChild { get; set; }
        public BinomialNode RightSibling { get; set; }

        // Constructor

        public BinomialNode(int item)
        {
            Item = item;
            Degree = 0;
            LeftMostChild = null;
            RightSibling = null;
        }
    }

    public class LazyBinomialHeap
    {
        private List<BinomialNode> B;
        private int highestPriorityIndex;
        private int maxDegree;

        public LazyBinomialHeap()
        {
            B = new List<BinomialNode>();
            highestPriorityIndex = -1;
            maxDegree = 1;
        }

        // Add method
        public void Add(int item)
        {
            // append the node at the beginning of B
            B.Insert(0, new BinomialNode(item));
            // record the index of the highestPriority node
            if (highestPriorityIndex == -1 || item > B[highestPriorityIndex].Item)
            {
                highestPriorityIndex = B.Count - 1;
            }
        }

        public BinomialNode Front()
        {
            // return the node with highestPriority
            if (highestPriorityIndex >= 0)
            {
                return B[highestPriorityIndex];
            }
            // return null if no node present
            return null;
        }

        public void Remove()
        {
            // only remove when B is not empty
            if (B.Count > 0)
            {
                // store the node to be deleted as q
                BinomialNode q = B[highestPriorityIndex];
                BinomialNode p = q.LeftMostChild;
                // remove the node with the highest priority
                B.RemoveAt(highestPriorityIndex);

                // break down p and insert back to B
                while (p != null)
                {
                    q = p.RightSibling;

                    p.RightSibling = null;
                    B.Add(p);
                    p = q;
                }
                // Coalesce B
                Coalesce();
            }
            else
            {
                Console.WriteLine("Current heap is empty already!");
            }
        }

        // TODO implement
        public void Print()
        {

        }

        private void Coalesce()
        {
            //
            int maxDepth = maxDegree > B.Count ? maxDegree + 1 : B.Count * 2;
            List<BinomialNode> newHeap = new List<BinomialNode>();
            for (int i = 0; i < maxDepth; i++)
            {
                newHeap.Add(null);
            }
            int newMaxDegree = 0;
            // loop over the original binomial heap
            for (int i = 0; i < B.Count; i++)
            {
                int degree = B[i].Degree;
                // if newHeap[degree] is empty, insert B[i] directly
                if (newHeap[degree] == null)
                {
                    newHeap[degree] = B[i];
                }
                // newHeap[degree] is occupied, merge newHeap[degree] with B[i]
                else
                {
                    BinomialNode newNode = mergeNode(B[i], newHeap[degree]);
                    // remove newHeap[degree] since it has been merged with B[i], new degree should be +1
                    newHeap[degree] = null;
                    // newHeap of degree + 1 may have been occupied already
                    // if is occupied, merge recursively until a newHeap[degree] is empty
                    while (newHeap[++degree] != null)
                    {
                        newNode = mergeNode(newNode, newHeap[degree]);
                        newHeap[degree] = null;
                    }
                    newHeap[degree] = newNode;
                }
                // keep recording the maxDegree of newHeap
                newMaxDegree = Math.Max(newMaxDegree, degree);
            }
            List<BinomialNode> tempHeap = new List<BinomialNode>();
            int highestPriorityIndex = 0;
            // remove empty slot of newHeap and form tempHeap
            for (int i = 0; i < newHeap.Count; i++)
            {
                // insert into tempHeap if newHeap[i] is not empty
                if (newHeap[i] != null)
                {
                    tempHeap.Add(newHeap[i]);
                    // keep recording the index of the highest priority node in tempHeap
                    if (highestPriorityIndex != 0 && newHeap[i].Item > tempHeap[highestPriorityIndex].Item)
                    {
                        highestPriorityIndex = i;
                    }
                }
            }
            // assign tempHeap as the new B
            B = tempHeap;
            maxDegree = newMaxDegree;
        }

        private BinomialNode mergeNode(BinomialNode p, BinomialNode q)
        {
            // logic same as the example code in lecture
            if (p.Item > q.Item)
            {
                q.RightSibling = p.LeftMostChild;
                p.LeftMostChild = q;
                p.Degree++;
                return p;
            }
            else
            {
                p.RightSibling = q.LeftMostChild;
                q.LeftMostChild = p;
                q.Degree++;
                return q;
            }
        }
    }

}

