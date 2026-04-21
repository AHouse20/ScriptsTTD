using UnityEngine;

public class DiceAnchor : MonoBehaviour
{
    [HideInInspector] public Dice diceReference;
    private void Start()
    {
        diceReference = GetComponentInParent<Dice>();
    }
}
