using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GridSystemVisual : SingletonMonobehaviour<GridSystemVisual>
{
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow
    }

    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;

    [SerializeField] 

    private GridSystemVisualSingle[,] gridSystemVisualSingles;

    private void Start()
    {
        gridSystemVisualSingles = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for(int z = 0 ; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualSingles[x,z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged += OnselectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += OnAnyUnitMovedGridPosition;
        UpdateGridVisual();
    }

    private void OnAnyUnitMovedGridPosition(object sender, Unit e)
    {
        UpdateGridVisual();
    }

    private void OnselectedActionChanged(object sender, ActionSelectedEventArgs e)
    {
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        foreach (GridSystemVisualSingle g in gridSystemVisualSingles)
        {
            g.Hide();
        }
    }

    public void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition > gridPositions = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x,z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                // Circular Range
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositions.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositions, gridVisualType);
    }
    public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach(GridPosition gridPosition in gridPositions)
        {
            gridSystemVisualSingles[gridPosition.x, gridPosition.z].
                Show(GetGridVisualMaterial(gridVisualType));
        }
    }
    private void UpdateGridVisual()
    {
        HideAllGridPositions();
        if(UnitActionSystem.Instance.GetSelectedAction() == null) return;
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        if (selectedAction.GetType() == typeof(AttackAction)) 
        {
            AttackAction attackAction = (AttackAction)selectedAction;
            ShowGridPositionRange(selectedUnit.GetGridPosition(), attackAction.GetRange(), GridVisualType.RedSoft);
        }
        if(selectedAction.GetType() == typeof(Ability))
        {
            Ability ability = (Ability)selectedAction;

        }
        ShowGridPositionList(selectedAction.GetValidActionGridPositions(), selectedAction.GetGridVisualType());
    }

    private Material GetGridVisualMaterial(GridVisualType gridVisualType)
    {
        foreach(GridVisualTypeMaterial g in gridVisualTypeMaterials)
        {
            if(g.gridVisualType == gridVisualType)
            {
                return g.material;
            }
        }

        Debug.LogError("Missing Material for GridVisualType " +  gridVisualType);
        return null;
    }
}
