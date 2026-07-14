using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Direction GetDirection(GridPosition sourceGridPosition, GridPosition targetGridPosition)
    {
        Vector2Int directionVector = new Vector2Int(targetGridPosition.x - sourceGridPosition.x,
            targetGridPosition.z - sourceGridPosition.z);

        if (directionVector.x == 0 && directionVector.y > 0)
        {
            return Direction.South;
        }
        else if (directionVector.x > 0 && directionVector.y > 0)
        {
            return Direction.SouthWest;
        }
        else if (directionVector.x > 0 && directionVector.y == 0)
        {
            return Direction.West;
        }
        else if (directionVector.x > 0 && directionVector.y < 0)
        {
            return Direction.NorthWest;
        }
        else if (directionVector.x == 0 && directionVector.y < 0)
        {
            return Direction.North;
        }
        else if (directionVector.x < 0 && directionVector.y < 0)
        {
            return Direction.NorthEast;
        }
        else if (directionVector.x < 0 && directionVector.y == 0)
        {
            return Direction.East;
        }
        else if (directionVector.x < 0 && directionVector.y > 0)
        {
            return Direction.SouthEast;
        }
        else
        {
            return Direction.North;
        }
    }

    public static bool[,] RotateBools(bool[,] bools, Direction direction, int range)
    {
        bool[,] rotatedBools = new bool[range, range];
        switch (direction)
        {
            case Direction.North:
                rotatedBools = bools;
                break;
            case Direction.East:
                rotatedBools = RotateClockwise(bools, range);
                break;
            case Direction.South:
                rotatedBools = RotateClockwise(bools, range);
                rotatedBools = RotateClockwise(rotatedBools, range);
                break;
            case Direction.West:
                rotatedBools = RotateClockwise(bools, range);
                rotatedBools = RotateClockwise(rotatedBools, range);
                rotatedBools = RotateClockwise(rotatedBools, range);
                break;
            default:
                rotatedBools = bools;
                break;
        }
        return rotatedBools;
    }

    public static bool[,] RotateClockwise(bool[,] bools, int range)
    {
        bool[,] rotatedBools = new bool[range, range];
        for (int i = range - 1; i >= 0; --i)
        {
            for (int j = 0; j < range; ++j)
            {
                rotatedBools[j, range - 1 - i] = bools[i, j];
            }
        }
        return rotatedBools;
    }

    public static void AddDescendants(Transform parent, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            list.Add(child.gameObject);
            AddDescendants(child, list);
        }
    }
}
