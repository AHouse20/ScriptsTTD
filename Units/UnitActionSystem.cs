using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;

public class UnitActionSystem : SingletonMonobehaviour<UnitActionSystem>
{
    public event EventHandler<UnitSelectedEventArgs> OnSelectedUnitChanged;
    public event EventHandler<ActionSelectedEventArgs> OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;


    [SerializeField] private Unit selectedUnit;
    private BaseAction selectedAction;
    [SerializeField] private LayerMask unitLayerMask;

    InputAction selectAction;
    InputAction altSelectAction;
    
    private bool isBusy;

    private void Start()
    {
        selectAction = InputSystem.actions.FindAction("Select");
        altSelectAction = InputSystem.actions.FindAction("AltSelect");

        TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
        
        SetSelectedUnit(selectedUnit);
    }

    private void OnDestroy()
    {
        TurnSystem.Instance.OnTurnChanged -= OnTurnChanged;
    }
    private void OnTurnChanged(object sender, TurnChangedEventArgs args)
    {
        if (!args.isPlayerTurn)
        {
            SetSelectedUnit(null);
        }
    }

    private void Update()
    {
        if (isBusy) return;

        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        if (EventSystem.current.currentSelectedGameObject != null) return;

        if(TryUnitSelection()) return;

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if(selectedAction == null) return;
        if(!selectedAction.CanBeUsed()) return;
        if (selectAction.WasCompletedThisFrame())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(CursorManager.GetMouseWorldPosition());

            if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                BaseActionParams actionParams = new BaseActionParams() { targetGridPosition = mouseGridPosition };
                selectedAction.TakeAction(actionParams, ClearBusy);
            }
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, true);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, false);
    }

    private bool TryUnitSelection()
    {
        if (selectAction.WasCompletedThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if(unit == selectedUnit) return false;

                    if (unit.IsEnemy()) return false;
 
                    if(!unit.IsSelectable()) return false;

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        if (selectedUnit != null)
        {
            SetSelectedAction(unit.GetAction<MoveAction>());
        }
        else
        {
            SetSelectedAction(null);
        }
        OnSelectedUnitChanged?.Invoke(this, new UnitSelectedEventArgs { unit = unit });
    }

    public void SetSelectedAction(BaseAction action)
    {
        selectedAction = action;

        OnSelectedActionChanged?.Invoke(this, new ActionSelectedEventArgs { baseAction = action });
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
}

public class UnitSelectedEventArgs
{
    public Unit unit;
}

public class ActionSelectedEventArgs
{
    public BaseAction baseAction;
}