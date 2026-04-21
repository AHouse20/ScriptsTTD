using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class AoeMatrix : SerializedMonoBehaviour
{
    [SerializeField, Range(0f, 5f)]
    private int aoe = 0;
    [SerializeField, HideInInspector]
    private int aoeChangeCheck = 0;
    [SerializeField, HideInInspector]
    private bool hasAoe = false;

    [ShowIf("hasAoe"), TableMatrix(SquareCells = true, DrawElementMethod = "DrawCell")]
    public bool[,] aoeTiles = new bool[3, 3];

    [ShowIf("hasAoe"), Button]
    private void FillAll()
    {
        for(int i = 0; i < aoe*2+1; i++)
        {
            for (int j = 0; j < aoe*2+1; j++)
            {
                aoeTiles[i, j] = true;
            }
        }
    }
    private void OnValidate()
    {
        if(aoeChangeCheck != aoe)
        {
            if(aoe == 0)
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
    }
    public static bool DrawCell(Rect rect, bool value)
    {
        if(Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            value = !value;
            GUI.changed = true;
            Event.current.Use();
        }
        EditorGUI.DrawRect(rect.Padding(5),
            value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));

        return value;
    }
}
