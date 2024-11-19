using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTiles
{
    private int width;
    private int length;
    private int[,] gridArray;
    private float gridSpace;
    private LayerMask walkableGround;
    public GridTiles(int width, int length, float gridSpace, LayerMask walkableGround, GameObject objType, Vector3 gridPositions) 
    {
        this.width = width;
        this.length = length;
        this.gridSpace = gridSpace;
        this.walkableGround = walkableGround;

        gridArray = new int[width, length];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                SpawnGrid.GenerateTiles(null, new Vector3(gridPositions.x + x * gridSpace, gridPositions.y + 1, gridPositions.z + z * gridSpace), walkableGround, objType);

            }
        }
    }
}
