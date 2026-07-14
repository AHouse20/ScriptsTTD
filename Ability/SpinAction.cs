using UnityEngine;
using System;
using System.Collections.Generic;

public class SpinAction : BaseAction
{

    private Vector3 startRotationVector;
    private float rotatedAmount;
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        float spinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        rotatedAmount += spinAmount;
        if(rotatedAmount >= 360f)
        {
            ActionComplete();
            rotatedAmount = 0;
            transform.eulerAngles = startRotationVector;
        }
       
    }

    public override void TakeAction(BaseActionParams actionParams, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        startRotationVector = transform.eulerAngles;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
    public override List<GridPosition> GetValidActionGridPositions()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override AIAction GetAIAction(GridPosition gridPosition)
    {
        return new AIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
