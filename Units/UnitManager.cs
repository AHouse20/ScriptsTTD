using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : SingletonMonobehaviour<UnitManager>
{
    private List<Unit> unitList;
    private List<Unit> friendlyList;
    private List<Unit> enemyList;

    protected override void Awake()
    {
        base.Awake();
        unitList = new List<Unit>();
        friendlyList = new List<Unit>();
        enemyList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += OnAnyUnitDead;
    }

    private void OnAnyUnitSpawned(object sender, Unit unit)
    {

        unitList.Add(unit);
        if (unit.IsEnemy())
        {
            enemyList.Add(unit);
        }
        else
        {
            friendlyList.Add(unit);
        }
    }
    private void OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        
        unitList.Remove(unit);
        if (unit.IsEnemy())
        {
            enemyList.Remove(unit);
        }
        else
        {
            friendlyList.Remove(unit);
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetFriendlyList()
    {
        return friendlyList;
    }
    public List<Unit> GetEnemyList()
    {
        return enemyList;
    }
}
