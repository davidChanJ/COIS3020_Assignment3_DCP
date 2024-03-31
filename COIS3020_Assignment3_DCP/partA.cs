using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS3020_Assignment3_DCP
{
    public enum
    public class partA
    {
        //Using an array of coordinates

        //Think in binary when calculating the index

        F
    }

    public class Point  //Used via Point quadtrees by Dave Mount
    {

    }

    public class QuadTreeNode()
    {
         
    }

    QuadTreeNode Insert(Point x, QuadTreeNode p, int cutDim )
    {
        if (p == null)
            p = new QuadTreeNode(x, cutDim);
        else if (p.point.equals(x))
            throw Exception("duplicate point");
        else if (p.inLeftSubtree(x))
            p.left = Insert(x, p.left, p(cutDim + 1) % x.getDim());
        else
            p.right = Insert(x, p.right, (p.cutDim + 1) % x.getDim());

        return p;
    }
}
