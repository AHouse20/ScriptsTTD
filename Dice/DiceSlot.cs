using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceSlot : MonoBehaviour, IPointerDownHandler
{
    private Dice hoveredDie;
    private Dice slottedDie;
    public Transform diceHolder;
    private bool slotted = false;
    [SerializeField] private EventReference slotSound;
    public IntEvent OnSlotted = new IntEvent();
    public IntEvent OnUnSlotted = new IntEvent();
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if (other != null && other.CompareTag("Dice"))
        {
            Dice diceRef = other.GetComponent<DiceAnchor>().diceReference;
            if(!diceRef.isDragging) return;
            diceRef.slotReference = this;
            hoveredDie = diceRef;
            GetComponent<Image>().color = Color.green;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited");
        if (other != null && other.CompareTag("Dice"))
        {
            Dice diceRef = other.GetComponent<DiceAnchor>().diceReference;
            diceRef.slotReference = null;
            hoveredDie = null;
        }
        GetComponent<Image>().color = Color.white;
    }

    public void SlotDie(Dice dieToSlot)
    {
        if (slotted)
        {
            UnSlotDie();
        }
        slotted = true;
        slottedDie = dieToSlot;
        dieToSlot.slotReference = this;
        AudioManager.Instance.PlayOneShot(slotSound, this.transform.position);
        GetComponent<Image>().color = Color.white;
        OnSlotted.Invoke(slottedDie.landedFace.value);
    }

    public void UnSlotDie()
    {
        if (!slotted) return;
        OnUnSlotted.Invoke(slottedDie.landedFace.value);
        slottedDie.ReturnObject();
        slotted = false;
        slottedDie = null;
    }

    public void ConsumeDie()
    {
        if (!slotted) return;
        OnUnSlotted.Invoke(slottedDie.landedFace.value);
        slottedDie.ReturnObject();
        PlayerDeckController.Instance.DiscardDie(slottedDie);
        slotted = false;
        slottedDie = null;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(slottedDie != null)
        {
            UnSlotDie();
        }
    }

    private void OnDisable()
    {
        OnSlotted.RemoveAllListeners();
        OnUnSlotted.RemoveAllListeners();
    }

}
