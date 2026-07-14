using System;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjectArray;
    private bool[,] walkableMap;
    public GridSystem(int width, int height, float cellSize, Func<GridSystem, GridPosition, GridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }
        }
        walkableMap = CreateWalkableMap();

        LevelGrid.Instance.OnAnyUnitMovedGridPosition += UpdateWalkableMap;
        Unit.OnAnyUnitSpawned += UpdateWalkableMap;
    }

    private void UpdateWalkableMap(object sender, Unit e)
    {
        if (e.BlocksMovement())
        {
            walkableMap = CreateWalkableMap();
        }
    }

    private bool[,] CreateWalkableMap()
    {
        walkableMap = new bool[width,height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                walkableMap[x, z] = gridObjectArray[x, z].IsWalkable();
            }
        }
        return walkableMap;
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        );
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebug gridDebug = debugTransform.GetComponent<GridDebug>();
                gridDebug.SetGridObject(GetGridObject(gridPosition) as GridObject);
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && 
               gridPosition.z >= 0 &&
               gridPosition.x < width &&
               gridPosition.z < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public bool[,] GetWalkableMap()
    {

        return walkableMap;
    }
}
