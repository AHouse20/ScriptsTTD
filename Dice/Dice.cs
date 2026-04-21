using FMODUnity;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dice : Draggable
{
    [System.Serializable]
    public struct DieFace
    {
        public int value;
        public Sprite face;
    }
    public DieFace[] faces;
    public float rollTime;
    public float rollInterval;
    private Image image;
    public DieFace landedFace;
    private Vector3 defaultPosition;
    [HideInInspector] public DiceSlot slotReference;

    public EventReference rollSound;
    public EventReference landedSound;

    [Button]
    public void Roll()
    {
        StartCoroutine("StartRoll");
    }

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        defaultPosition = Vector3.zero;
    }

    private IEnumerator StartRoll()
    {
        AudioManager.Instance.PlayOneShot(rollSound, transform.position);
        float stopTime = Time.time + rollTime;
        int faceToLand = Random.Range(0, faces.Length);
        while (Time.time < stopTime)
        {
            image.sprite = faces[Random.Range(0, faces.Length)].face;
            image.transform.localPosition = new Vector3(
                Random.Range(defaultPosition.x -1, defaultPosition.x +1),
                Random.Range(defaultPosition.y -1, defaultPosition.y +1),
                defaultPosition.z);
            yield return new WaitForSeconds(rollInterval);
        }
        LandFace(faceToLand);
        yield return null;
    }

    private void LandFace(int faceNumber)
    {
        AudioManager.Instance.PlayOneShot(landedSound, transform.position);
        image.transform.localPosition = defaultPosition;
        landedFace = faces[faceNumber];
        image.sprite = landedFace.face;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if(slotReference != null)
        {
            isDragging = false;
            movingPiece.transform.localScale = Vector3.one;
            movingPiece.transform.SetParent(slotReference.diceHolder, false);
            movingPiece.transform.localPosition = Vector3.zero;
            slotReference.SlotDie(this);
            targetPosition = Vector3.zero;
        }
        else
        {
            base.OnPointerUp(eventData);
        }

    }
}
