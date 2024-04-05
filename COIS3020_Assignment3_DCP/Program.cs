
using COIS3020_Assignment3_DCP;
using DavidDataStructureAssignment3;

class Program
{
    static void Main(string[] args)
    {
        TestQuadTree();

        TestLazyBinomialHeap();

        TestTwoThreeFourTree();
    }

    static void TestQuadTree()
    {
        int[] value = { 1, 1, 1, 1, 1 };
        QuadTree originalTree = new QuadTree(value);

        int[] value2 = { 2, 0, 3, -6, 4 };
        QuadTree otherTree = new QuadTree(value2);

        int direction = QuadTree.PointDirection(originalTree, otherTree);
        Console.WriteLine("Direction = " + direction);
    }

    static void TestLazyBinomialHeap()
    {
        LazyBinomialHeap root = new LazyBinomialHeap();
        root.Add(3);
        root.Add(6);
        root.Add(1);
        root.Add(34);
        root.Add(5);

        Console.WriteLine("Current front of lazy binomial heap is: " + root.Front()?.Item);

        root.Add(99);
        Console.WriteLine("Front of lazy binomial heap after inserting 99 is: " + root.Front()?.Item);

        root.Add(2);
        Console.WriteLine("Front of lazy binomial heap after inserting 2 is: " + root.Front()?.Item);

        root.Remove();


        // TODO implement
        root.Print();
    }

    static void TestTwoThreeFourTree()
    {
        TwoThreeFourTree<int> root = new TwoThreeFourTree<int>();

        root.Insert(1);
        root.Insert(2);
        root.Insert(3);
        root.Insert(4);
        root.Insert(5);
        root.Insert(6);
        root.Insert(7);
        root.Insert(8);
        root.Insert(9);
        root.Insert(10);

        //TODO implement
        root.Print();

        root.Delete(10);

        root.Print();
    }
}