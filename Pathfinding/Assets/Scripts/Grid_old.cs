using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
    public enum Terraintype {PLAIN,BLOCKED,HARD}
    public int width = 64;
    public int height = 64;

    public Terraintype[,] grid;
    private bool generated = false;

    public void Awake()
    {

        grid = new Terraintype[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = Terraintype.PLAIN;
            }
        }

        for (int y = 0; y < height; y++)
        {
            float rand = Random.Range(0.15f, 0.2f);
            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0.0f, 1.0f) < rand)
                {
                    grid[x, y] = Terraintype.BLOCKED;
                }
                if (Random.Range(0.0f, 1.0f) < rand)
                {
                    grid[x, y] = Terraintype.HARD;
                }
            }
        }
        generated = true;
    }

    private void OnDrawGizmos()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] == (int)Terraintype.PLAIN)
                {

                    Gizmos.color = Color.white;
                }
                else if(grid[x,y] ==Terraintype.BLOCKED)
                {
                    Gizmos.color = Color.black;
                }
                else if (grid[x, y] == Terraintype.HARD)
                {
                    Gizmos.color = Color.gray;
                }
                Gizmos.DrawCube(new Vector3(x, y, 0), new Vector3(1, 1));
            }
        }
    }
}
