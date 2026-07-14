using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    private List<Vector3> targetPositions;
    private int currentPositionIndex;
    private bool unitMoving = false;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] private int maxStamina = 3;
    private int stamina = 3;
    public event EventHandler<bool> OnMovingChanged;

    protected override void Awake()
    {
        base.Awake();
        stamina = maxStamina;
    }

    protected override void Start()
    {
        base.Start();
        TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
    }

    private void OnDestroy()
    {
        TurnSystem.Instance.OnTurnChanged -= OnTurnChanged;
    }
    private void OnTurnChanged(object sender, TurnChangedEventArgs e)
    {
        stamina = maxStamina;
    }

    public override void TakeAction(BaseActionParams actionParams, Action onActionComplete)
    {
        List<GridPosition> path = Pathfinding.Instance.FindPath(unit.GetGridPosition(), actionParams.targetGridPosition);

        currentPositionIndex = 0;
        targetPositions = new List<Vector3>();

        foreach (GridPosition gridPosition in path)
        {
            targetPositions.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        stamina -= path.Count-1;

        OnMovingChanged?.Invoke(this, true);
        ActionStart(onActionComplete);
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        Vector3 targetPosition = targetPositions[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * turnSpeed);
        transform.position += moveDirection * Time.deltaTime * moveSpeed;

        float proximity = Vector3.Distance(transform.position, targetPosition);
        if(proximity < 0.1)
        {
            currentPositionIndex++;
            if(currentPositionIndex >= targetPositions.Count)
            {
                OnMovingChanged?.Invoke(this, false);
                ActionComplete();
            }
        }
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        List<GridPosition > validGridPositions = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -stamina; x <= stamina; x++)
        {
            for(int z = -stamina; z <= stamina; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > stamina)
                {
                    continue;
                }
                if(Pathfinding.Instance.FindPath(unitGridPosition, testGridPosition).Count > stamina+1) continue;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if(unitGridPosition == testGridPosition) continue;
                if(LevelGrid.Instance.HasAnyUnit(testGridPosition)) continue; 

                validGridPositions.Add(testGridPosition);
            }
        } 

        return validGridPositions;

    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override AIAction GetAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<AttackAction>().GetTargetCountAtPosition(gridPosition);
        return new AIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
