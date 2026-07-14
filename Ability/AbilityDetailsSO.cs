using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New AbilityDetails", menuName = "Scriptable Objects/AbilityDetails")]
public class AbilityDetailsSO : SerializedScriptableObject
{
    public string abilityName;
    public Color abilityColor;

    public int usesPerTurn = 1;
    public int usesPerCombat = -1;

    //public List<DiceSlot> slots;
    //public List<AbilityFunction> functions;

    #region Range
    [SerializeField, Range(0f, 8f)]
    public int range = 0;
    [SerializeField, HideInInspector]
    private int rangeChangeCheck = 0;
    [SerializeField, HideInInspector]
    private bool hasRange = false;

    [ShowIf("hasRange"), TableMatrix(SquareCells = true, DrawElementMethod = "DrawCellRange")]
    public bool[,] rangeTiles = new bool[3, 3];

    [ShowIf("hasRange"), Button("Fill All")]
    private void FillAllRange()
    {
        for (int i = 0; i < range * 2 + 1; i++)
        {
            for (int j = 0; j < range * 2 + 1; j++)
            {
                rangeTiles[i, j] = true;
            }
        }
    }
#if UNITY_EDITOR
    public static bool DrawCellRange(Rect rect, bool value)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = !value;
            GUI.changed = true;
            Event.current.Use();
        }
        EditorGUI.DrawRect(rect.Padding(5),
            value ? new Color(0.1f, 0.2f, 0.8f) : new Color(0, 0, 0, 0.5f));

        return value;
    }
#endif
#endregion

    #region AOE
    [SerializeField, Range(0f, 5f)]
    public int aoe = 0;
    [SerializeField, HideInInspector]
    private int aoeChangeCheck = 0;
    [SerializeField, HideInInspector]
    private bool hasAoe = false;

    [ShowIf("hasAoe"), TableMatrix(SquareCells = true, DrawElementMethod = "DrawCellAoe")]
    public bool[,] aoeTiles = new bool[3, 3];

    [ShowIf("hasAoe"), Button]
    private void FillAll()
    {
        for (int i = 0; i < aoe * 2 + 1; i++)
        {
            for (int j = 0; j < aoe * 2 + 1; j++)
            {
                aoeTiles[i, j] = true;
            }
        }
    }
    private void OnValidate()
    {
        if (aoeChangeCheck != aoe)
        {
            if (aoe == 0)
            {
                hasAoe = false;
            }
            else
            {
                hasAoe = true;
            }
            aoeTiles = new bool[aoe * 2 + 1, aoe * 2 + 1];
            aoeTiles[aoe, aoe] = true;
            aoeChangeCheck = aoe;
        }
        if (rangeChangeCheck != range)
        {
            if (range == 0)
            {
                hasRange = false;
            }
            else
            {
                hasRange = true;
            }
            rangeTiles = new bool[range * 2 + 1, range * 2 + 1];
            rangeTiles[range, range] = true;
            rangeChangeCheck = range;
        }
    }
#if UNITY_EDITOR
    public static bool DrawCellAoe(Rect rect, bool value)
    {
        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = !value;
            GUI.changed = true;
            Event.current.Use();
        }
        EditorGUI.DrawRect(rect.Padding(5),
            value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));

        return value;
    }
#endif
#endregion
}