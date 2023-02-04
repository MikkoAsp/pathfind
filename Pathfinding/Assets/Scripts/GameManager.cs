using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject SlabModel = null;

    public GameObject GridModel = null;

    private GridPath gridpath = null;

    private GameObject[,] slabs = null;

    public Color NormalColor = Color.white;
    public Color RouteColor = Color.blue;
    public Color StartColor = Color.green;
    public Color EndColor = Color.red;



    // Start is called before the first frame update
    void Start()
    {

        gridpath = GridModel.GetComponent<GridPath>();
        gridpath.Dijkstra();

        slabs = new GameObject[gridpath.width, gridpath.height];

        int xoffset = gridpath.width / 2;
        int yoffset = gridpath.height / 2;

        foreach (Node n in gridpath.visited)
        {
            GameObject gobj = Instantiate(SlabModel, new Vector3(10*(n.x - xoffset), 10*(n.y - yoffset), 0), SlabModel.transform.rotation);
            // store this slab
            slabs[n.x, n.y] = gobj;
            // Set the cost for this slab
            gobj.GetComponent<Slab>().setCost(n.cost);
            // Set x and y
            gobj.GetComponent<Slab>().setCoords(n.x, n.y);

            foreach (Node p in gridpath.path)
            {
                if (p == n)
                {
                    gobj.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                }
            }

            if (n.x == gridpath.start_x && n.y == gridpath.start_y)
            {
                gobj.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }

            if (n.x == gridpath.end_x && n.y == gridpath.end_y)
            {
                gobj.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }

        }



    }

    // Update is called once per frame
    void Update()
    {
        if (gridpath.NewPathGenerated)
        {

            foreach (Node n in gridpath.visited)
            {
                GameObject gobj = slabs[n.x, n.y];
                gobj.GetComponent<Slab>().setCost(n.cost);

                gobj.GetComponentInChildren<SpriteRenderer>().color = NormalColor;

                foreach (Node p in gridpath.path)
                {
                    if (p == n)
                    {
                        gobj.GetComponentInChildren<SpriteRenderer>().color = RouteColor;
                    }
                }
                if (n.x == gridpath.start_x && n.y == gridpath.start_y)
                {
                    gobj.GetComponentInChildren<SpriteRenderer>().color = StartColor;
                }

                if (n.x == gridpath.end_x && n.y == gridpath.end_y)
                {
                    gobj.GetComponentInChildren<SpriteRenderer>().color = EndColor;
                }

            }


            gridpath.NewPathGenerated = false;
        }
    }
}
