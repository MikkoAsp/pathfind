using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int x;
    public int y;
    public int cost;
    public Node parent;
    public int gCost;
    public int hCost;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(int _x, int _y){
        x = _x; y = _y;
        parent = null;
        cost = System.Int32.MaxValue;  // should be infinity...
    }

}
