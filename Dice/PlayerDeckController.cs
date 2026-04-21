using NUnit.Framework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TurnBasedStrategyFramework.Common.Controllers;
using TurnBasedStrategyFramework.Unity.Controllers;
using TurnBasedStrategyFramework.Unity.Players;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeckController : SingletonMonobehaviour<PlayerDeckController>
{
    [SerializeField] private Transform handParent;
    [SerializeField] private Transform drawParent;
    [SerializeField] private Transform discardParent;
    [SerializeField] private float drawDelay;
    private GameManager gameManager;
    private UnityGridController gridController;
    [SerializeField] private Dice dicePrefab;

    private List<Dice> handDice = new List<Dice>();
    private List<Dice> drawDice = new List<Dice>();
    private List<Dice> discardDice = new List<Dice>();

    public List<Dice> initialDice = new List<Dice>();

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
        gridController = gameManager.gridController;
    }

    private void OnEnable()
    {
        gridController.TurnStarted += OnTurnStart;
        gridController.TurnEnded += OnTurnEnd;
    }

    private void OnDisable()
    {
        gridController.TurnStarted -= OnTurnStart;
        gridController.TurnEnded -= OnTurnEnd;
    }

    private void Start()
    {
        foreach (Dice dice in initialDice)
        {
            Dice newDie = Instantiate(dice, drawParent);
            drawDice.Add(newDie);
        }
    }
    private void OnTurnStart(TurnTransitionParams @params)
    {
        if(@params.TurnContext.CurrentPlayer == FindAnyObjectByType<UnityPlayerManager>().GetPlayerByNumber(0))
        {
            Debug.Log("TurnStart");
            DrawHand();
        }
    }
    private void OnTurnEnd(TurnTransitionParams @params)
    {
        if (@params.TurnContext.CurrentPlayer == FindAnyObjectByType<UnityPlayerManager>().GetPlayerByNumber(0))
        {
            Debug.Log("TurnEnd");
            DiscardHand();
        }
    }
    [Button]
    private void DrawHand()
    {
        DiscardHand();
        StartCoroutine(DrawDice(5));
    }

    private void DiscardHand()
    {
        while (handDice.Count > 0)
        {
            DiscardDie(handDice[0]);
        }
    }

    public void DiscardDie(Dice die)
    {
        die.transform.SetParent(discardParent, false);
        discardDice.Add(die);
        handDice.Remove(die);
    }
    private IEnumerator DrawDice(int drawAmount)
    {
        int diceDrawn = 0;
        while(diceDrawn < drawAmount)
        {
            DrawDie();
            diceDrawn++;
            yield return new WaitForSeconds(drawDelay);
        }
        yield return null;
    }

    private void DrawDie()
    {
        if(drawDice.Count > 0)
        {
            Dice dieToDraw = drawDice[0];
            handDice.Add(dieToDraw);
            dieToDraw.transform.SetParent(handParent, false);
            dieToDraw.Roll();
            drawDice.RemoveAt(0);
        }
        else
        {
            ReshuffleDeck();
            DrawDie();
        }
    }

    private void ReshuffleDeck()
    {
        while(discardDice.Count > 0)
        {
            discardDice[0].transform.SetParent(drawParent, false);
            drawDice.Add(discardDice[0]);
            discardDice.RemoveAt(0);
        }
        drawDice = drawDice.OrderBy(x => Random.value).ToList();
    }
}
