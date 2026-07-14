using System;
using System.Collections.Generic;
using UnityEngine;

public class Ability : BaseAction
{
    [SerializeField] private AbilityDetailsSO abilityDetails;
    public override string GetActionName()
    {
        return abilityDetails.abilityName;
    }

    public override AIAction GetAIAction(GridPosition gridPosition)
    {
        throw new NotImplementedException();
    }

    public override List<GridPosition> GetValidActionGridPositions()
    {
        throw new NotImplementedException();
    }

    public override void TakeAction(BaseActionParams actionParams, Action onActionComplete)
    {
        throw new NotImplementedException();
    }

    public AbilityDetailsSO GetAbilityDetails()
    {
        return abilityDetails;
    }
}
