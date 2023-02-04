using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GridPath : MonoBehaviour
{

    public enum TerrainType {PLAIN, BLOCKED, HARD};

    private static int MAX_GRID_SIZE = 128;
    [Range(1, 32)]
    public int width = 32;
    [Range(1, 32)]
    public int height = 32;


    [Range(0, 31)]
    public int start_x = 1;
    [Range(0, 31)]
    public int start_y = 1;
    [Range(0, 31)]
    public int end_x = 12;
    [Range(0, 31)]
    public int end_y = 8;

    private int temp_start_x, temp_start_y, temp_end_x, temp_end_y;

    public List<Node> path = new List<Node>();
    public List<Node> visited = new List<Node>();

    public TerrainType[,] grid = null;
    //private bool Generated = false;

    public bool NewPathGenerated = false;

    /*
    private void OnValidate()
    {
        GenerateGrid();
        
    }
    */
    private void Start()
    {
        Dijkstra();
        temp_start_x = start_x;
        temp_start_y = start_y;
        temp_end_x = end_x;
        temp_end_y = end_y;
    }
    private void Update()
    {
        if(temp_start_x !=start_x || temp_start_y !=start_y || temp_end_x != end_x || temp_end_y != end_y)
        {
            Dijkstra();
            temp_start_x = start_x;
            temp_start_y = start_y;
            temp_end_x = end_x;
            temp_end_y = end_y;
        }
    }
    public void Dijkstra()
    {
        // list of unvisited nodes --> should be something more efficient
        List<Node> unvisited = new List<Node>();

        // list of visited nodes
        //List<Node> visited = new List<Node>();
        visited.Clear();
        

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != TerrainType.BLOCKED)
                    unvisited.Add(new Node(x, y));
            }
        }

        // Find the start node:
        Node current = unvisited.Find(n => n.x == start_x && n.y == start_y);
        if (current == null)
        {
            Debug.Log("Start node not found!");
            return;
        }
        // Start node cost --> 0
        current.cost = 0;

        // loop until:
        // - end node has been visited OR
        // - minimum distance in unvisited is Int32.MaxValue

        while (unvisited.Count > 0)
        {
            //Debug.Log("Unvisited count: " + unvisited.Count);

            // go through the neighbors of current node
            for (int x = current.x - 1; x <= current.x + 1; x++)
            {
                for (int y = current.y - 1; y <= current.y + 1; y++)
                {

                    // check that we are NOT in the current position
                    if (x == current.x & y == current.y)
                        continue;

                    // Find the neighbour having x and y coords
                    Node neighbour = unvisited.Find(n => n.x == x && n.y == y);
                    if (neighbour == null)  // Not found...
                        continue;

                    // Get the current cost
                    int cost = current.cost;
                    // If both coords (x,y) are different
                    if (neighbour.x != current.x && neighbour.y != current.y)
                    {
                        cost += 14;
                    }
                    else
                    {
                        cost += 10;
                    }

                    // if cost is smaller than current cost, update
                    if (cost < neighbour.cost)
                    {
                        neighbour.cost = cost;
                        neighbour.parent = current;
                    }
                }
            } // end for

            // remove current from unvisited and add to visited
            visited.Add(current);
            unvisited.Remove(current);

            // This was the last element...
            if (unvisited.Count == 0)
            {
                break;
            }

            // find minimum from unvisited
            int minvalue = unvisited.Min(n => n.cost);
            current = unvisited.Find(n => n.cost == minvalue);

            if (grid[end_x, end_y] == TerrainType.BLOCKED)
            {
                Debug.Log("no end path because its blocked");
                return;
            }
            // the minimum is System.Int32.MaxValue --> no route
            if (current.cost == System.Int32.MaxValue)
            {
                Debug.Log("NO ROUTE TO END NODE!");
                break;
            }

            // Set boolean
            NewPathGenerated = true;

        } 
        // try to get the path...
        path.Clear();
        current = visited.Find(n => n.x == end_x && n.y == end_y);
        path.Add(current);
        while (!(current.x == start_x && current.y == start_y))
        {
            current = current.parent;
            path.Add(current);
        }
        path.Reverse();

    } // end Dijkstra()
    void Awake()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
        
        grid = new TerrainType[MAX_GRID_SIZE, MAX_GRID_SIZE];

        for (int y=0; y<height; y++)
        {
            for (int x=0; x<width; x++)
            {
                grid[x, y] =  TerrainType.PLAIN;
            }
        }

        // Block random parts of the grid
        for (int y = 0; y < height; y++)
        {
            float rand = Random.Range(0.1f, 0.2f);

            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0.0f, 1.0f) < rand)
                {
                    grid[x, y] = TerrainType.BLOCKED;   
                }
            }
        }

        /*
        for (int y = 0; y < height; y++)
        {
            float rand = Random.Range(0.05f, 0.1f);

            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0.0f, 1.0f) < rand)
                {
                    grid[x, y] = TerrainType.HARD;
                }
            }
        }
        */

        //Generated = true;
    }

    

    void FooOnDrawGizmos()
    {
        if (grid == null)
            GenerateGrid();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] == TerrainType.PLAIN)
                    Gizmos.color = Color.white;
                else if (grid[x, y] == TerrainType.BLOCKED)
                    Gizmos.color = Color.black;
                else if (grid[x, y] == TerrainType.HARD)
                    Gizmos.color = Color.gray;

                Gizmos.DrawCube(transform.position+new Vector3(x,y,0), new Vector3(1, 1, 1));
            }
        }

        // Test our pathfinding:
        Dijkstra();

        // Draw the visited nodes with cost as color???
        float maxcost = visited.Max(n => n.cost);
        //Debug.Log(maxcost);
        foreach (Node n in visited)
        {
            Gizmos.color = new Color( 1.0f, 1.0f - n.cost/maxcost, 0);
            Gizmos.DrawSphere(transform.position + new Vector3(n.x, n.y, 0), 0.3f);
        }


        // Draw start and end points
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + new Vector3(start_x, start_y, 0), 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(end_x, end_y, 0), 0.2f);


        // Draw the path
        foreach (Node n in path)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + new Vector3(n.x, n.y, 0), 0.1f);
        }
    }

}
