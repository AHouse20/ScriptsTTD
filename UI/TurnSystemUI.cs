using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.GPUSort;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    private void Start()
    {
        endTurnButton.onClick.AddListener(TurnSystem.Instance.NextTurn);
        TurnSystem.Instance.OnTurnChanged += OnTurnChanged;
    }

    private void OnTurnChanged(object sender, TurnChangedEventArgs args)
    {
        UpdateTurnText(args.turnNumber);
        UpdateEndTurnButtonStatus(args.isPlayerTurn);
    }

    private void UpdateEndTurnButtonStatus(bool isPlayerTurn)
    {
        endTurnButton.gameObject.SetActive(isPlayerTurn);
    }

    private void UpdateTurnText(int turnNumber)
    {
        turnNumberText.text = "Turn " + turnNumber;
    }
}
