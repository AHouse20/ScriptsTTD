using TurnBasedStrategyFramework.Common.Cells;
using TurnBasedStrategyFramework.Common.Units;
using UnityEngine.Events;

public class IntEvent : UnityEvent<int> { }
public class AbilityAddedEvent : UnityEvent<DiceAbility> { }
public class AbilityUsedEvent : UnityEvent<DiceAbility, AbilityUsedEventArgs> { }

public class AbilityUsedEventArgs
{
    public ICell targetCell;
    public IUnit targetUnit;
}