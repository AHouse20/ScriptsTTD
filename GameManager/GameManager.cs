using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TurnBasedStrategyFramework.Common.Cells;
using TurnBasedStrategyFramework.Common.Controllers;
using TurnBasedStrategyFramework.Unity.Units;
using TurnBasedStrategyFramework.Unity.Cells;
using UnityEditor;
using UnityEngine;
using TurnBasedStrategyFramework.Unity.Controllers;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    [SerializeField] private List<CharacterDetails> heroes = new List<CharacterDetails>();
    [SerializeField] private List<CharacterDetails> enemies = new List<CharacterDetails>();
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform unitParent;
    public CursorManager cursor;
    public UnityGridController gridController;
    [Button]
    private void CreateCombat(IGridController gridController)
    {
        if (gridController == null) gridController = this.gridController;
        List<ICell> cells = gridController.CellManager.GetCells().ToList();
        for (int i = 0; i < 3; i++)
        {
            List<ICell> validCells = cells.Where(c => c.IsTaken == false && c.GridCoordinates.x > 4 && c.GridCoordinates.y > 4).ToList();
            Cell selectedCell = (Cell)validCells[Random.Range(0, validCells.Count)];

            CreateUnit(heroes[i], selectedCell, 0);
        }

        for (int i = 0; i < 3; i++)
        {
            List<ICell> validCells = cells.Where(c => c.IsTaken == false && c.GridCoordinates.x < 4 && c.GridCoordinates.y < 4).ToList();
            Cell selectedCell = (Cell)validCells[Random.Range(0, validCells.Count)];

            CreateUnit(enemies[i], selectedCell, 1);
        }
        gridController.StartGame();
    }

    public void CreateUnit(UnitDetails unitDetails, Cell selectedCell, int playerNumber)
    {
        Character newUnit = (PrefabUtility.InstantiatePrefab(unitPrefab.gameObject) as GameObject).GetComponent<Character>();
        newUnit.PlayerNumber = playerNumber;
        newUnit.CurrentCell = selectedCell;

        selectedCell.IsTaken = true;
        selectedCell.CurrentUnits.Add(newUnit);

        newUnit.transform.position = selectedCell.transform.position;
        newUnit.transform.parent = unitParent.transform;
        newUnit.transform.rotation = selectedCell.transform.rotation;

        GameObject.FindAnyObjectByType<UnityUnitManager>().AddUnit(newUnit);

        newUnit.Initialise(unitDetails);
    }
}
