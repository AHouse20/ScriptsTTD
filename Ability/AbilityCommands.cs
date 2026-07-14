using FMOD;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/*
public struct CommandArgs
{
    public IUnit UnitReference;
    public IEnumerable<ICell> cells;
    public List<IUnit> units;
    public int value;
    public List<AbilityFunction> functions;
}

[System.Serializable]
public class AbilityFunction
{
    [SerializeReference] private AbilityAction action;
    public Task Execute(CommandArgs args, IGridController gridController)
    {
        action.Execute(args, gridController);
        return Task.CompletedTask;
    }

    public Task Undo(CommandArgs args, IGridController gridController)
    {
        action.Undo(args, gridController);
        return Task.CompletedTask;
    }
}

public class AbilityAction
{
    public virtual Task Execute(CommandArgs args, IGridController gridController)
    {
        return Task.CompletedTask;
    }
    public virtual Task Undo(CommandArgs args, IGridController gridController)
    {
        return Task.CompletedTask;
    }
}



class MasterCommand : ICommand
{
    public readonly CommandArgs args;

    public MasterCommand(CommandArgs args)
    {
        this.args = args;
    }

    public ICommand Deserialize(Dictionary<string, object> actionParams, IGridController gridController)
    {
        return null;
    }

    public Task Execute(IUnit unit, IGridController controller)
    {
        foreach(AbilityFunction function in args.functions)
        {
            function.Execute(args, controller);
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

class Damage : AbilityAction
{
    public bool usesValue;
    [ShowIf("usesValue")] public float valueMultiplier;
    public int flatDamage;
    public override Task Execute(CommandArgs args, IGridController gridController)
    {
        foreach (var target in args.units.Where(u => u.PlayerNumber != args.UnitReference.PlayerNumber))
        {
            target.ModifyHealth(-args.value, args.UnitReference);
        }
        return Task.CompletedTask;
    }

    public override Task Undo(CommandArgs args, IGridController controller)
    {
        foreach (var target in args.units.Where(u => u.PlayerNumber != args.UnitReference.PlayerNumber))
        {
            target.ModifyHealth(args.value, args.UnitReference);
        }
        return Task.CompletedTask;
    }
}

class CreateUnit : AbilityAction
{
    public UnitDetails unit;
    List<IUnit> spawnedUnits;
    public override Task Execute(CommandArgs args, IGridController gridController)
    {
        foreach (ICell cell in args.cells.Where(c => !c.IsTaken))
        {
            GameManager.Instance.CreateUnit(unit, cell as Cell, args.UnitReference.PlayerNumber);
        }
        return Task.CompletedTask;
    }

    public override Task Undo(CommandArgs args, IGridController controller)
    {
        foreach (IUnit obstacle in spawnedUnits)
        {
            obstacle.RemoveFromGame();
        }
        return Task.CompletedTask;
    }
}
*/