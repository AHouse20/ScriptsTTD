using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;
    private bool isWalkable;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        SetWalkable(true);
        unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += unit + "\n";
        }
        return gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
        CheckWalkable();
    }

    private bool CheckWalkable()
    {
        foreach (Unit unit in unitList)
        {
            if (unit.BlocksMovement())
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
        CheckWalkable();
    }
    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return unitList[0];
        }
        return null;
    }

    public bool IsWalkable()
    {
        if (unitList.Count > 0 && unitList[0].BlocksMovement())
        {
            return false;
        }
        return isWalkable;
    }

    public void SetWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }
}
