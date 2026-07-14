using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatDetails", menuName = "Scriptable Objects/CombatDetails")]
public class CombatDetails : ScriptableObject
{
    [System.Serializable]
    public struct Wave
    {
        public List<UnitDetails> units;
    }
    public List<Wave> waves;
}
