using System;
using System.Collections.Generic;
using UnityEngine;

public class OnUnitMovedEventArgs
{
    public Unit unit;
    public GridPosition fromGridPosition;
    public GridPosition toGridPosition;
}

public class LevelGrid : SingletonMonobehaviour<LevelGrid>
{
    public event EventHandler<Unit> OnAnyUnitMovedGridPosition;
    [SerializeField] private Transform gridDebugPrefab;
    private GridSystem gridSystem;

    [SerializeField] int width;
    [SerializeField] int height;
    protected override void Awake()
    {
        base.Awake();
        gridSystem = new GridSystem(width, height, 5f, (GridSystem g, GridPosition gridPosition) => new GridObject(g,gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugPrefab);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitsAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit = null)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        if(unit == null)
        {
            unit = gridObject.GetUnit();
        }
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        OnAnyUnitMovedGridPosition.Invoke(this, unit);
    }
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();

    public bool[,] GetWalkableMap() => gridSystem.GetWalkableMap();

    public bool HasAnyUnit(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
}

