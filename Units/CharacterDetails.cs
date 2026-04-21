using FMODUnity;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class UnitDetails : SerializedScriptableObject
{
    public string characterName;
    public GameObject characterModel;
    public AnimatorOverrideController animator;
    public EventReference readySound;
    public EventReference hurtSound;

    public int maxHealth;
}

[CreateAssetMenu(fileName = "CharacterDetails", menuName = "Scriptable Objects/CharacterDetails")]
public class CharacterDetails : UnitDetails
{
    public int stamina;

    public List<AbilityDetailsSO> startingAbilities;
}
