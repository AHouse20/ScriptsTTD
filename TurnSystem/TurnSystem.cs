using System;
using UnityEngine;

public class TurnSystem : SingletonMonobehaviour<TurnSystem>
{

    public EventHandler<TurnChangedEventArgs> OnTurnChanged;
    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;

        TurnChangedEventArgs args = new TurnChangedEventArgs()
        {
            turnNumber = this.turnNumber,
            isPlayerTurn = this.isPlayerTurn
        };
        OnTurnChanged?.Invoke(this, args);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}

public class TurnChangedEventArgs : EventArgs
{
    public int turnNumber;
    public bool isPlayerTurn;
}