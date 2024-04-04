using COIS3020_Assignment3_DCP;

public class Program
{
    static void Main(string[] args)
    {
        TestQuadTree();

        TestLazyBinomialHeap();
    }

    static void TestQuadTree()
    {
        Console.WriteLine("Testing Quad Tree -- Part A \n");
        int[] value = { 1, 1, 1, 1, 1 };
        QuadTree originalTree = new QuadTree(value);

        int[] value2 = { 2, 0, 3, -6, 4 };
        QuadTree otherTree = new QuadTree(value2);

        int direction = QuadTree.PointDirection(originalTree, otherTree);
        Console.WriteLine("Direction = " + direction);
    }

    static void TestLazyBinomialHeap()
    {
        Console.WriteLine("Testing Lazy Binomial Heap -- Part B \n");
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
        //Test insert

        //Test delete parts

        //Test search

        //Test other methods

        //How abou to make Convert() and Print() in part C?
        //Thank you,
        //from David.
    }
}