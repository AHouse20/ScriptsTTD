using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using System;

public class Unit : Entity
{
    public static event EventHandler<Unit> OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private Transform actionCameraTransform;
    [SerializeField] private bool isEnemy;

    private BaseAction[] baseActions;

    private bool isSelectable = true;
    protected override void Awake()
    {
        base.Awake();
        baseActions = GetComponents<BaseAction>();
    }

    protected override void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }
    protected override void Start()
    {
        base.Start();
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += OnTurnChanged;

        OnAnyUnitSpawned?.Invoke(this, this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        TurnSystem.Instance.OnTurnChanged -= OnTurnChanged;
    }
    protected override void OnDeath(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        isSelectable = false;
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    private void OnTurnChanged(object sender, TurnChangedEventArgs args)
    {
        if ((isEnemy && !args.isPlayerTurn) || (!isEnemy && args.isPlayerTurn))
        {
            // TurnStarted
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public BaseAction[] GetBaseActions()
    {
        return baseActions;
    }

    public Transform GetActionCameraTransform()
    {
        return actionCameraTransform;
    }

    public bool CanPerform(BaseAction action)
    {
        return action.CanBeUsed();
    }

    public bool IsSelectable()
    {
        return isSelectable;
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction action in baseActions)
        {
            if(action is T)
            {
                return (T)action;
            }
        }
        return null;
    }

}
