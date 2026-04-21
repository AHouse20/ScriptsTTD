using TurnBasedStrategyFramework.Unity.Units;
using UnityEngine;

public class TTDUnit : Unit
{
    [SerializeField] protected Transform modelParent;
    protected GameObject model;
    public virtual void Initialise(UnitDetails unitDetails)
    {
        gameObject.name = unitDetails.characterName;
        model = Instantiate(unitDetails.characterModel, modelParent);
        model.transform.localPosition = Vector3.zero;
        MaxHealth = unitDetails.maxHealth;
        Health = MaxHealth;
    }
}
