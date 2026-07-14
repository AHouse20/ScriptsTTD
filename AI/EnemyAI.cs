using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForTurn,
        TakingTurn,
        Busy
    }

    private State state;    

    private float timer;
    private void Awake()
    {
        state = State.WaitingForTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
    }

    private void OnTurnChanged(object sender, TurnChangedEventArgs e)
    {
        if (!e.isPlayerTurn)
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    private bool TryTakeAIAction(Action onAIActionComplete)
    {
        foreach(Unit unit in UnitManager.Instance.GetEnemyList())
        {
            if(TryTakeAIAction(unit, onAIActionComplete)) return true;
        }
        return false;
    }

    private bool TryTakeAIAction(Unit unit, Action onAIActionComplete)
    {
        AIAction bestAIAction = null;
        BaseAction bestAction = null;
        foreach(BaseAction action in unit.GetBaseActions())
        {
            if(!unit.CanPerform(action)) continue;
            if(bestAIAction == null)
            {
                bestAIAction = action.GetBestAIAction();
                bestAction = action;
            }
            else
            {
                AIAction testAIAction = action.GetBestAIAction();
                if(testAIAction != null && testAIAction.actionValue > bestAIAction.actionValue)
                {
                    bestAIAction = action.GetBestAIAction();
                    bestAction = action;
                }
            }
        }
        if(bestAIAction != null && unit.CanPerform(bestAction))
        {
            BaseActionParams actionParams = new BaseActionParams() { targetGridPosition = bestAIAction.gridPosition };
            bestAction.TakeAction(actionParams, onAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn()) return;

        switch (state) 
        { 
            case State.WaitingForTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryTakeAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        //No more enemy actions
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }

        

    }
}
