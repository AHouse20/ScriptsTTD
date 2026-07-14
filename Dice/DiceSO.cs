using UnityEngine;

[CreateAssetMenu(fileName = "New Die", menuName = "Scriptable Objects/Dice")]
public class DiceSO : ScriptableObject
{
    public DieFace[] faces = new DieFace[6];
}

[System.Serializable]
public struct DieFace
{
    public int value;
    public Sprite face;
}
