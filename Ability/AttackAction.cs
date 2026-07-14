using FMODUnity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackActionArgs : EventArgs
{
    public AttackAction.State state;
    public Unit targetUnit;
    public Unit sourceUnit;
    public Action onHit;
}

public class AttackAction : BaseAction
{

    [SerializeField] private int damage = 5;
    public enum State
    {
        Inactive,
        Windup,
        Active,
        Backswing
    }
    [SerializeField] private int maxRange = 3;
    [SerializeField] private GridSystemVisual.GridVisualType rangeGridVisualType;
    private float stateTimer;
    private State state;
    private Unit targetUnit;
    private bool canAttack;
    private float turnSpeed = 10f;

    [SerializeField] private EventReference windupSound;
    [SerializeField] private EventReference activeSound;
    [SerializeField] private EventReference backswingSound;

    public event EventHandler<AttackActionArgs> OnStateChanged;

    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Windup:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Slerp(transform.forward, aimDirection, Time.deltaTime * turnSpeed);
                break;
            case State.Active:
                if (canAttack)
                {
                    //Attack();
                    canAttack = false;
                }
                break;
            case State.Backswing:
                break;

        }

        if(stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void Attack()
    {
        targetUnit.Damage(damage);
        OnImpact();
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Windup:
                state = State.Active;
                if(!activeSound.IsNull) AudioManager.Instance.PlayOneShot(activeSound, transform.position);
                float activeStateTime = 0.3f;
                stateTimer = activeStateTime;
                break;
            case State.Active:
                state = State.Backswing;
                if (!backswingSound.IsNull) AudioManager.Instance.PlayOneShot(backswingSound, transform.position);
                float backswingStateTime = 0.5f;
                stateTimer = backswingStateTime;
                break;
            case State.Backswing:
                ActionComplete();
                break;
        }
        OnStateChanged?.Invoke(this, new AttackActionArgs
        {
            state = this.state,
            targetUnit = targetUnit,
            sourceUnit = unit,
            onHit = Attack
        });
    }
    public override string GetActionName()
    {
        return "Attack";
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositions(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositions(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        for (int x = -maxRange; x <= maxRange; x++)
        {
            for (int z = -maxRange; z <= maxRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                // Circular Range
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxRange)
                {
                    continue;
                }

                //
                /* Show full Range
                validGridPositions.Add(testGridPosition);
                continue;
                */
                if (gridPosition == testGridPosition) continue;
                if (!LevelGrid.Instance.HasAnyUnit(testGridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if(!targetUnit.IsAttackable()) continue;
                if(!targetUnit.IsTargetable()) continue;
                if(targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                validGridPositions.Add(testGridPosition);
            }
        }
        return validGridPositions;
    }

    public override void TakeAction(BaseActionParams actionParams, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        state = State.Windup;
        if (!windupSound.IsNull) AudioManager.Instance.PlayOneShot(windupSound, transform.position);
        OnStateChanged?.Invoke(this, new AttackActionArgs
        {
            state = this.state,
            targetUnit = targetUnit,
            sourceUnit = unit
        });
        float windupStateTime = 1f;
        stateTimer = windupStateTime;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(actionParams.targetGridPosition);

        canAttack = true;
    }

    protected override void ActionSelected()
    {

    }

    protected override void ActionDeselected()
    {
        /*
        Debug.Log("Deselected!");
        state = State.Inactive;
        OnStateChanged?.Invoke(this, new AttackActionArgs
        {
            state = this.state
        });
        base.ActionDeselected();
        */
    }

    public override AIAction GetAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        if(!targetUnit.IsAttackable()) return new AIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        }; ;
        return new AIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1-targetUnit.GetHealthNormalized()) * 100),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositions(gridPosition).Count;
    }

    public int GetRange()
    {
        return maxRange;
    }
}
