using System.Collections.Generic;
using UnityEngine;
using AStar;

public class Pathfinding : SingletonMonobehaviour<Pathfinding>
{

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        (int, int)[] path = AStarPathfinding.GeneratePathSync(startGridPosition, endGridPosition);
        List<GridPosition> gridPositionPath = new List<GridPosition>();
        foreach((int,int) pathCoord in path)
        {
            GridPosition newPosition = new GridPosition(pathCoord.Item1, pathCoord.Item2);
            gridPositionPath.Add(newPosition);
        }
        //PrintPath(gridPositionPath);
        return gridPositionPath;
    }

    private void PrintPath(List<GridPosition> path)
    {
        string output = "";

        foreach (GridPosition cordinate in path)
        {
            output += $"({cordinate.x}, {cordinate.z}), ";
        }

        Debug.Log(output);
    }
}
