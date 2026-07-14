using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseActionParams { public GridPosition targetGridPosition; }
public class BaseActionImpactArgs { public float screenShakeAmount; }
public class OnAnyActionStartedEventArgs { public bool useActionCamera; public Transform actionCameraTransform; }
public class OnAnyActionCompleteEventArgs { public bool useActionCamera; }
public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler<OnAnyActionStartedEventArgs> OnAnyActionStarted;

    public static event EventHandler<OnAnyActionCompleteEventArgs> OnAnyActionComplete;

    public static event EventHandler<BaseActionImpactArgs> OnAnyActionImpact;

    [SerializeField] private GridSystemVisual.GridVisualType gridVisualType;
    [SerializeField] private bool limitedUsesPerTurn = true;
    [SerializeField, ShowIf("limitedUsesPerTurn")] private int maxUsesPerTurn = 1;
    [SerializeField] private bool limitedUsesPerCombat = false;
    [SerializeField, ShowIf("limitedUsesPerCombat")] private int maxUsesPerCombat = -1;
    [SerializeField] private float screenShakeAmount;
    [SerializeField] private bool useActionCamera;
    [SerializeField] private AbilityDetailsSO abilityDetails;
    protected Action onActionComplete;
    protected Unit unit;
    protected bool isActive;
    protected int usesThisTurn;
    protected int usesThisCombat;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    protected virtual void Start()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;
        TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedActionChanged -= OnSelectedActionChanged;
        TurnSystem.Instance.OnTurnChanged -= OnTurnChanged;
    }
    private void OnSelectedActionChanged(object sender, ActionSelectedEventArgs e)
    {
        if(e.baseAction == this && e.baseAction.GetUnit() == this.unit)
        {
            ActionSelected();
        }
        else
        {
            ActionDeselected();
        }
    }
    private void OnTurnChanged(object sender, TurnChangedEventArgs e)
    {
        usesThisTurn = 0;
    }
    public abstract string GetActionName();

    public abstract void TakeAction(BaseActionParams actionParams, Action onActionComplete);

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositions();
        return validGridPositions.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositions();

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStartedEventArgs args = new()
        {
            useActionCamera = this.useActionCamera,
            actionCameraTransform = unit.GetActionCameraTransform()
        };
        OnAnyActionStarted?.Invoke(this, args);
    }

    protected void ActionComplete()
    {
        usesThisTurn++;
        usesThisCombat++;
        isActive = false;
        onActionComplete();
        OnAnyActionCompleteEventArgs args = new()
        {
            useActionCamera = this.useActionCamera
        };
        OnAnyActionComplete?.Invoke(this, args);
    }

    protected virtual void OnImpact()
    {
        BaseActionImpactArgs args = new()
        {
            screenShakeAmount = this.screenShakeAmount,
        };
        OnAnyActionImpact?.Invoke(this, args);
    }
    protected virtual void ActionSelected()
    {

    }

    protected virtual void ActionDeselected()
    {

    }

    public Unit GetUnit()
    {
        return unit;
    }

    public GridSystemVisual.GridVisualType GetGridVisualType()
    {
        return gridVisualType;
    }

    public bool CanBeUsed()
    {
        if(limitedUsesPerCombat && usesThisCombat >= maxUsesPerCombat)
        {
            return false;
        }
        else if(limitedUsesPerTurn && usesThisTurn >= maxUsesPerTurn)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public AIAction GetBestAIAction()
    {
        List<AIAction> possibleActions = new List<AIAction>();
        List<GridPosition> validGridPositions = GetValidActionGridPositions();

        foreach(GridPosition gridPosition in validGridPositions)
        {
            AIAction AIAction = GetAIAction(gridPosition);
            possibleActions.Add(AIAction);
        }

        if(possibleActions.Count <= 0) return null;

        possibleActions.Sort((AIAction a, AIAction b) => b.actionValue - a.actionValue);

        return possibleActions[0];
    }

    public abstract AIAction GetAIAction(GridPosition gridPosition);
}

