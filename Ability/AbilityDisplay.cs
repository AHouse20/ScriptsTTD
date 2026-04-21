using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using TurnBasedStrategyFramework.Common.Controllers;
using TurnBasedStrategyFramework.Common.Controllers.GridStates;
using TurnBasedStrategyFramework.Unity.Units.Abilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityDisplay : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform slotsParent;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private DiceSlot slotPrefab;
    [SerializeField] private Image backdrop;
    private string baseDesc = "Deal {0} Damage";
    public List<DiceSlot> activeSlots = new List<DiceSlot>();
    public DiceAbility abilityRef;

    private void Start()
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(abilityRef.CurrentValue > 0)
        {
            GameManager.Instance.gridController.GridState = new GridStateUnitSelected(abilityRef.UnitReference, abilityRef);
        }
    }

    private void Update()
    {
        SetDescription(0);
    }

    public void SetDescription(int value)
    {
        string desc = string.Format(baseDesc, abilityRef.CurrentValue);
        descriptionText.text = desc;
    }
    [Button]
    public void Initialise(DiceAbility ability)
    {
        this.abilityRef = ability;
        while (activeSlots.Count > 0)
        {
            Destroy(activeSlots[0].gameObject);
            activeSlots.RemoveAt(0);
        }
        foreach (DiceSlot slot in abilityRef.slots)
        {
            DiceSlot newSlot = Instantiate(slot, slotsParent);
            activeSlots.Add(newSlot);
        }
        nameText.text = abilityRef.abilityName;
        backdrop.color = abilityRef.abilityColor;
        abilityRef.OnAbilityUsed.AddListener(ConsumeDice);
        ability.InitDisplay(this);
    }

    private void OnDisable()
    {
        abilityRef.OnAbilityUsed.RemoveListener(ConsumeDice);
    }
    public void UnslotDice()
    {
        foreach(DiceSlot slot in activeSlots)
        {
            slot.UnSlotDie();
        }
    }

    private void ConsumeDice(DiceAbility ability, AbilityUsedEventArgs args)
    {
        Debug.Log("Consume");
        foreach (DiceSlot slot in activeSlots)
        {
            slot.ConsumeDie();
        }
    }
}
