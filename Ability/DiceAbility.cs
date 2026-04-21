using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TbsFramework;
using TurnBasedStrategyFramework.Common.Cells;
using TurnBasedStrategyFramework.Common.Controllers;
using TurnBasedStrategyFramework.Common.Controllers.GridStates;
using TurnBasedStrategyFramework.Common.Units;
using TurnBasedStrategyFramework.Common.Units.Abilities;
using TurnBasedStrategyFramework.Unity.Gui;
using TurnBasedStrategyFramework.Unity.Units;
using TurnBasedStrategyFramework.Unity.Units.Abilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class DiceAbility : Ability
{
    public AbilityUsedEvent OnAbilityUsed = new AbilityUsedEvent();

    public AbilityDetailsSO abilityDetails;
    public AbilityDisplay displayReference;

    public string abilityName;
    public Color abilityColor;

    public List<DiceSlot> slots = new List<DiceSlot>();

    private int currentValue;

    public int CurrentValue {  get { return currentValue; } }

    public Character characterReference;
    public void Initialise(AbilityDetailsSO abilityDetails)
    {
        this.abilityDetails = abilityDetails;
        abilityName = abilityDetails.abilityName;
        abilityColor = abilityDetails.abilityColor;
        aoe = abilityDetails.aoe;
        aoeTiles = abilityDetails.aoeTiles;
        abilityRange = abilityDetails.abilityRange;
        slots = new List<DiceSlot>();
        slots.AddRange(abilityDetails.slots);
    }

    public void InitDisplay(AbilityDisplay display)
    {
        foreach(DiceSlot slot in display.activeSlots)
        {
            slot.OnSlotted.AddListener(OnDiceSlotted);
            slot.OnUnSlotted.AddListener(OnDiceUnSlotted);
        }
    }

    private void OnDiceUnSlotted(int value)
    {
        currentValue -= value;
    }

    private void OnDiceSlotted(int value)
    {
        currentValue += value;
    }

    private int aoe = 2;
    private bool[,] aoeTiles;
    private int _damage = 10;    
    private Vector2Int abilityRange = new Vector2Int(0,5);
    private IEnumerable<ICell> _cellsInRange;
    private IEnumerable<ICell> _cellsInRadius;
    private IEnumerable<IUnit> _unitsInRadius;
    private bool isSelected = false;

    public void DeselectAbility()
    {
        GameManager.Instance.gridController.GridState = new GridStateUnitSelected(UnitReference, UnitReference.GetBaseAbilities());
    }

    public override void OnAbilityDeselected(IGridController gridController)
    {
        isSelected = false;
        PlayerControl.Instance.cancelEvent.RemoveListener(DeselectAbility);
    }
    public override void OnAbilitySelected(IGridController gridController)
    {
        isSelected = true;
        PlayerControl.Instance.cancelEvent.AddListener(DeselectAbility);
        _cellsInRange = gridController.CellManager.GetCells().Where(c => Enumerable.Range(abilityRange.x, abilityRange.y).Contains(c.GetDistance(UnitReference.CurrentCell)));
        gridController.CellManager.MarkAsInRange(_cellsInRange);
    }
    public override void OnCellHighlighted(ICell cell, IGridController gridController)
    {
        //_cellsInRadius = gridController.CellManager.GetCells().Where(c => c.GetDistance(cell) <= aoe);
        _cellsInRadius = FindCellsInRange(cell, gridController);
        gridController.CellManager.MarkAsReachable(_cellsInRadius);
        _unitsInRadius = _cellsInRadius.SelectMany(c => c.CurrentUnits).Where(u => u.PlayerNumber != UnitReference.PlayerNumber).ToList();
        gridController.UnitManager.MarkAsTargetable(_unitsInRadius);
    }

    public override void OnUnitHighlighted(IUnit unit, IGridController gridController)
    {
        OnCellHighlighted(unit.CurrentCell, gridController);
    }

    public override void OnCellClicked(ICell cell, IGridController gridController)
    {
        if (_cellsInRange.Contains(cell))
        {
            var unitsInRange = _cellsInRadius.SelectMany(c => c.CurrentUnits).Where(u => u.PlayerNumber != UnitReference.PlayerNumber).ToList();
            UnitReference.HumanExecuteAbility(new MultipleTargetAttackCommand(unitsInRange, currentValue), gridController);
            AbilityUsedEventArgs args = new AbilityUsedEventArgs() { targetCell = cell };
            OnAbilityUsed.Invoke(this, args);
        }
        else
        {
            DeselectAbility();
        }
    }

    public override void OnUnitClicked(IUnit unit, IGridController gridController)
    {
        OnCellClicked(unit.CurrentCell, gridController);
    }
    public override void OnCellDehighlighted(ICell cell, IGridController gridController)
    {
        CheckCells(_cellsInRadius, gridController);
        gridController.UnitManager.UnMark(_unitsInRadius);
    }

    public override void CleanUp(IGridController gridController)
    {
        gridController.CellManager.UnMark(_cellsInRange);
        gridController.CellManager.UnMark(_cellsInRadius);
        gridController.UnitManager.UnMark(_unitsInRadius);
    }

    private void CheckCells(IEnumerable<ICell> cells, IGridController gridController)
    {
        gridController.CellManager.UnMark(cells);
        gridController.CellManager.MarkAsInRange(cells.Where(c => _cellsInRange.Contains(c)));
    }

    private List<ICell> FindCellsInRange(ICell targetCell, IGridController gridController)
    {
        List<ICell> cellsInRange = gridController.CellManager.GetCells().Where(c => CheckInRange(c, targetCell)).ToList();
        return cellsInRange;
    }

    private bool CheckInRange(ICell cellToCheck, ICell sourceCell)
    {

        if(cellToCheck.GridCoordinates.x < sourceCell.GridCoordinates.x - aoe || cellToCheck.GridCoordinates.x > sourceCell.GridCoordinates.x + aoe)
        {
            return false;
        }
        else if (cellToCheck.GridCoordinates.y < sourceCell.GridCoordinates.y - aoe || cellToCheck.GridCoordinates.y > sourceCell.GridCoordinates.y + aoe)
        {
            return false;
        }
        else if (aoeTiles[cellToCheck.GridCoordinates.x - (sourceCell.GridCoordinates.x-aoe), cellToCheck.GridCoordinates.y - (sourceCell.GridCoordinates.y - aoe)] == false)
        {
            return false;
        }
        return true;
    }
    class MultipleTargetAttackCommand : ICommand
    {
        private readonly IEnumerable<IUnit> _targets;
        private readonly int _damage;

        public MultipleTargetAttackCommand(IEnumerable<IUnit> targets, int damage)
        {
            _targets = targets;
            _damage = damage;
        }

        public ICommand Deserialize(Dictionary<string, object> actionParams, IGridController gridController)
        {
            return null;
        }
        public Task Execute(IUnit unit, IGridController controller)
        {
            //controller.UnitManager.MarkAsAttacking(unit, _targets.First());

            foreach (var target in _targets)
            {
                target.ModifyHealth(-_damage, unit);
            }
            return Task.CompletedTask;
        }

        public Dictionary<string, object> Serialize()
        {
            return null;
        }

        public Task Undo(IUnit unit, IGridController controller)
        {
            return Task.CompletedTask;
        }
    }
}
