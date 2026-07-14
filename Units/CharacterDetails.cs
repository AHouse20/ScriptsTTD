using FMODUnity;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class UnitDetails : SerializedScriptableObject
{
    public string unitName;
    public GameObject unitModel;
    public float yOffset = 0;
    public float rotationCorrection = 0;
    public float scaleFactor = 1;
    public Vector3 cameraOffset = Vector3.zero;
    public AnimatorOverrideController animator;
    public int maxHealth;
}

[CreateAssetMenu(fileName = "CharacterDetails", menuName = "Scriptable Objects/CharacterDetails")]
public class CharacterDetails : UnitDetails
{
    public int stamina = 2;
    public int dice = 2;
    public List<AbilityDetailsSO> startingAbilities;
    public EventReference readySound;
    public EventReference hurtSound;
}


