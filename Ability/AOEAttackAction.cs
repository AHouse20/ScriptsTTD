using System;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttackAction : BaseAction
{
    [SerializeField] private Transform projectilePrefab;
    [SerializeField] private int damage = 5;
    [SerializeField] private int maxRange = 3;
    [SerializeField] private int AOE = 2;
    private GridPosition targetGridPosition;
    private BombProjectile bombProjectile;
    public override string GetActionName()
    {
        return "Bomb";
    }

    public override AIAction GetAIAction(GridPosition gridPosition)
    {
        return new AIAction { 
        gridPosition = gridPosition,
        actionValue = 0,
        };

    }

    private void Attack()
    {
        List<Unit> targets = new List<Unit>();
        List<GridPosition> positionsInAOE = new List<GridPosition>();
        for (int x = -AOE; x <= AOE; x++)
        {
            for (int z = -AOE; z <= AOE; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = targetGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                /* Circular Range
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxRange)
                {
                    continue;
                }
                */
                //
                /* Show full Range
                validGridPositions.Add(testGridPosition);
                continue;
                */
                positionsInAOE.Add(testGridPosition);
                if (!LevelGrid.Instance.HasAnyUnit(testGridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                Debug.Log(targetUnit);
                if (!targetUnit.IsAttackable()) continue;
                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;
                targets.Add(targetUnit);
            }
        }
        foreach(Unit targetUnit in targets)
        {
            targetUnit.Damage(damage);
        }
        OnImpact();
        ActionComplete();
    }
    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();

        GridPosition gridPosition = unit.GetGridPosition();

        for (int x = -maxRange; x <= maxRange; x++)
        {
            for (int z = -maxRange; z <= maxRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                // Circular Range
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxRange)
                {
                    continue;
                }

                //
                /* Show full Range
                validGridPositions.Add(testGridPosition);
                continue;
                */
                if (gridPosition == testGridPosition) continue;
                validGridPositions.Add(testGridPosition);
            }
        }
        return validGridPositions;
    }

    public override void TakeAction(BaseActionParams actionParams, Action onActionComplete)
    {
        Transform projectileInstance = Instantiate(projectilePrefab, unit.GetCenterOfMass().position, Quaternion.identity);
        bombProjectile = projectileInstance.GetComponent<BombProjectile>();
        bombProjectile.Setup(actionParams.targetGridPosition, Attack);
        targetGridPosition = actionParams.targetGridPosition;
        ActionStart(onActionComplete);
    }
}
